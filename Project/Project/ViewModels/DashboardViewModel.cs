using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Enums;
using Project.Models.Entities;
using Project.Models.Services;
using Project.Models.Services.Interfaces;

namespace Project.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly ITransactionService _transactionService;
    private readonly SessionService _session;

    public DashboardViewModel(ITransactionService transactionService, SessionService session)
    {
        _transactionService = transactionService;
        _session = session;
    }

    [ObservableProperty]
    public partial decimal Balance { get; set; }

    [ObservableProperty]
    public partial decimal IncomeThisMonth { get; set; }

    [ObservableProperty]
    public partial decimal ExpenseThisMonth { get; set; }

    [ObservableProperty]
    public partial decimal NetThisMonth { get; set; }

    [ObservableProperty]
    public partial decimal NetLastMonth { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNoRecentTransactions))]
    public partial ObservableCollection<TransactionDto> RecentTransactions { get; set; } = [];

    public bool HasNoRecentTransactions => RecentTransactions.Count == 0;

    public string Currency => _session.CurrentUser?.Currency ?? "EUR";
    public string Username => _session.CurrentUser?.Username ?? string.Empty;

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        var userId = _session.CurrentUser!.Id;
        var now = DateTime.Now;
        var last = now.AddMonths(-1);

        var balanceTask      = _transactionService.GetBalanceAsync(userId);
        var incomeNowTask    = _transactionService.GetMonthlyTotalAsync(userId, TransactionType.Income,  now.Year,  now.Month);
        var expenseNowTask   = _transactionService.GetMonthlyTotalAsync(userId, TransactionType.Expense, now.Year,  now.Month);
        var incomeLastTask   = _transactionService.GetMonthlyTotalAsync(userId, TransactionType.Income,  last.Year, last.Month);
        var expenseLastTask  = _transactionService.GetMonthlyTotalAsync(userId, TransactionType.Expense, last.Year, last.Month);
        var recentTask       = _transactionService.GetRecentAsync(userId, 10);

        await Task.WhenAll(balanceTask, incomeNowTask, expenseNowTask, incomeLastTask, expenseLastTask, recentTask);

        Balance          = balanceTask.Result;
        IncomeThisMonth  = incomeNowTask.Result;
        ExpenseThisMonth = expenseNowTask.Result;
        NetThisMonth     = incomeNowTask.Result  - expenseNowTask.Result;
        NetLastMonth     = incomeLastTask.Result - expenseLastTask.Result;
        RecentTransactions = new ObservableCollection<TransactionDto>(recentTask.Result);

        IsBusy = false;
    }
}
