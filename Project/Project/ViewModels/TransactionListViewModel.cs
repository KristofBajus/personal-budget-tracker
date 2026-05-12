using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Project.Models.Entities;
using Project.Models.Services;
using Project.Models.Services.Interfaces;
using Project.Views.Popups;

namespace Project.ViewModels;

public partial class TransactionListViewModel : BaseViewModel
{
    private readonly ITransactionService _transactionService;
    private readonly SessionService _session;

    public TransactionListViewModel(ITransactionService transactionService, SessionService session)
    {
        _transactionService = transactionService;
        _session = session;
    }

    [ObservableProperty]
    public partial ObservableCollection<TransactionDto> Transactions { get; set; } = [];

    [ObservableProperty]
    public partial string SearchText { get; set; } = string.Empty;

    partial void OnSearchTextChanged(string value) => ApplyFilters();

    private List<TransactionDto> _allTransactions = [];

    // --- Picker sources ---
    public List<string> TypeOptions { get; } = ["All types", "Income", "Expense"];

    public List<string> MonthOptions { get; } =
    [
        "All months",
        "January","February","March","April","May","June",
        "July","August","September","October","November","December"
    ];

    public List<string> YearOptions { get; } = BuildYearOptions();

    private static List<string> BuildYearOptions()
    {
        var years = new List<string> { "All years" };
        var current = DateTime.UtcNow.Year;
        for (var y = current; y >= current - 4; y--)
            years.Add(y.ToString());
        return years;
    }

    // --- Filter state (null = no filter) ---
    // String wrappers let the Picker show a Title placeholder when nothing is selected
    [ObservableProperty]
    public partial string? FilterTypeName { get; set; }

    [ObservableProperty]
    public partial string? FilterMonthName { get; set; }

    [ObservableProperty]
    public partial string? FilterYearName { get; set; }

    // Reload automatically whenever a filter changes
    partial void OnFilterTypeNameChanged(string? value) => _ = LoadAsync();
    partial void OnFilterMonthNameChanged(string? value) => _ = LoadAsync();
    partial void OnFilterYearNameChanged(string? value) => _ = LoadAsync();

    private int? MappedFilterYear => FilterYearName is null or "All years"
        ? null
        : int.Parse(FilterYearName);

    // Map string picker values to the types the service expects
    private TransactionType? MappedFilterType => FilterTypeName switch
    {
        "Income"  => TransactionType.Income,
        "Expense" => TransactionType.Expense,
        _         => null  // "All types" or null → no filter
    };

    private int? MappedFilterMonth => FilterMonthName is null or "All months"
        ? null
        : MonthOptions.IndexOf(FilterMonthName); // "January" is index 1 after "All months", so IndexOf gives the correct month number

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            _allTransactions = await _transactionService.GetFilteredAsync(
                _session.CurrentUser!.Id,
                MappedFilterType,
                null,
                MappedFilterMonth,
                MappedFilterYear);
            ApplyFilters();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ApplyFilters()
    {
        var filtered = _allTransactions.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(t =>
                (t.Note?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                t.CategoryName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        Transactions.Clear();
        foreach (var item in filtered)
            Transactions.Add(item);
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        var vm = IPlatformApplication.Current!.Services.GetRequiredService<AddEditTransactionViewModel>();
        await vm.InitForAddAsync();

        var popup = new AddEditTransactionPopup(vm);
        await Shell.Current.ShowPopupAsync(popup);

        if (vm.SaveSucceeded)
            await LoadAsync();
    }

    [RelayCommand]
    private async Task EditAsync(TransactionDto dto)
    {
        var vm = IPlatformApplication.Current!.Services.GetRequiredService<AddEditTransactionViewModel>();
        await vm.InitForEditAsync(dto);

        var popup = new AddEditTransactionPopup(vm);
        await Shell.Current.ShowPopupAsync(popup);

        if (vm.SaveSucceeded)
            await LoadAsync();
    }

    [RelayCommand]
    private async Task DeleteAsync(TransactionDto dto)
    {
        var confirmed = await Shell.Current.DisplayAlert(
            "Delete Transaction",
            $"Delete this {dto.Type.ToString().ToLower()} of {dto.FormattedAmount}?",
            "Delete", "Cancel");

        if (!confirmed) return;

        await _transactionService.DeleteAsync(dto.Id);
        await LoadAsync();
    }
}
