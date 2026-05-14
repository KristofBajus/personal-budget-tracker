using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Enums;
using Project.Models.Entities;
using Project.Models.Services;
using Project.Models.Services.Interfaces;

namespace Project.ViewModels;

public partial class AddEditTransactionViewModel : BaseViewModel
{
    private readonly ITransactionService _transactionService;
    private readonly ICategoryService _categoryService;
    private readonly SessionService _session;

    public AddEditTransactionViewModel(
        ITransactionService transactionService,
        ICategoryService categoryService,
        SessionService session)
    {
        _transactionService = transactionService;
        _categoryService = categoryService;
        _session = session;
    }

    // Watched by the popup code-behind to know when to close
    [ObservableProperty]
    public partial bool SaveSucceeded { get; set; }

    public bool IsEditing => EditingId is not null;
    public int? EditingId { get; private set; }

    public string PopupTitle => IsEditing ? "Edit Transaction" : "Add Transaction";

    // Source for the type picker
    public TransactionType[] TypeOptions { get; } = [TransactionType.Income, TransactionType.Expense];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    [ObservableProperty]
    public partial string AmountText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial DateTime Date { get; set; } = DateTime.UtcNow.Date;

    [ObservableProperty]
    public partial TransactionType SelectedType { get; set; } = TransactionType.Expense;

    [ObservableProperty]
    public partial ObservableCollection<CategoryDto> Categories { get; set; } = [];

    [ObservableProperty]
    public partial CategoryDto? SelectedCategory { get; set; }

    [ObservableProperty]
    public partial string Note { get; set; } = string.Empty;

    // Fires automatically when SelectedType changes — reloads categories for the new type
    partial void OnSelectedTypeChanged(TransactionType value)
    {
        SelectedCategory = null;
        ErrorMessage = string.Empty;
        _ = ReloadCategoriesAsync(value);
    }

    private async Task ReloadCategoriesAsync(TransactionType type)
    {
        try
        {
            await LoadCategoriesAsync(type);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public async Task InitForAddAsync()
    {
        EditingId = null;
        AmountText = string.Empty;
        Date = DateTime.UtcNow.Date;
        SelectedType = TransactionType.Expense;
        Note = string.Empty;
        ErrorMessage = string.Empty;
        SaveSucceeded = false;
        OnPropertyChanged(nameof(PopupTitle));
        await LoadCategoriesAsync(SelectedType);
    }

    public async Task InitForEditAsync(TransactionDto dto)
    {
        EditingId = dto.Id;
        AmountText = dto.Amount.ToString("N2");
        Date = dto.Date;
        SelectedType = dto.Type;
        Note = dto.Note ?? string.Empty;
        ErrorMessage = string.Empty;
        SaveSucceeded = false;
        OnPropertyChanged(nameof(PopupTitle));
        await LoadCategoriesAsync(dto.Type);
        // Set after loading so the picker has items to match against
        SelectedCategory = Categories.FirstOrDefault(c => c.Id == dto.CategoryId);
    }

    private async Task LoadCategoriesAsync(TransactionType type)
    {
        var all = await _categoryService.GetAllAsync(_session.CurrentUser!.Id);
        Categories = new ObservableCollection<CategoryDto>(
            all.Where(c => c.Type == type));
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        ErrorMessage = string.Empty;

        if (!decimal.TryParse(
                AmountText.Replace(',', '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var amount) || amount <= 0)
        {
            ErrorMessage = "Enter a valid amount greater than zero.";
            return;
        }

        if (SelectedCategory is null)
        {
            ErrorMessage = "Please select a category.";
            return;
        }

        IsBusy = true;
        try
        {
            if (IsEditing)
                await _transactionService.UpdateAsync(
                    EditingId!.Value, amount, Date, SelectedType, SelectedCategory.Id, Note);
            else
                await _transactionService.AddAsync(
                    _session.CurrentUser!.Id, amount, Date, SelectedType, SelectedCategory.Id, Note);

            SaveSucceeded = true;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
