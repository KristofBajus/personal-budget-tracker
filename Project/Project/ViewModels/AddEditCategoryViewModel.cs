using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Enums;
using Project.Models.Entities;
using Project.Models.Services;
using Project.Models.Services.Interfaces;

namespace Project.ViewModels;

public partial class AddEditCategoryViewModel : BaseViewModel
{
    private readonly ICategoryService _categoryService;
    private readonly SessionService _session;

    public AddEditCategoryViewModel(ICategoryService categoryService, SessionService session)
    {
        _categoryService = categoryService;
        _session = session;
    }

    public List<string> PresetColors { get; } =
    [
        "#EF4444", "#F97316", "#EAB308", "#22C55E",
        "#14B8A6", "#3B82F6", "#8B5CF6", "#EC4899",
        "#6B7280", "#0EA5E9", "#10B981", "#F43F5E",
        "#A855F7", "#84CC16", "#FB923C", "#64748B"
    ];

    public TransactionType[] TypeOptions { get; } = [TransactionType.Income, TransactionType.Expense];

    [ObservableProperty]
    public partial bool SaveSucceeded { get; set; }

    public bool IsEditing => EditingId is not null;
    public int? EditingId { get; private set; }

    public string PopupTitle => IsEditing ? "Edit Category" : "Add Category";

    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial TransactionType SelectedType { get; set; } = TransactionType.Expense;

    [ObservableProperty]
    public partial string SelectedColor { get; set; } = "#3B82F6";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public void InitForAdd()
    {
        EditingId = null;
        Name = string.Empty;
        SelectedType = TransactionType.Expense;
        SelectedColor = PresetColors[0];
        ErrorMessage = string.Empty;
        SaveSucceeded = false;
        OnPropertyChanged(nameof(PopupTitle));
        OnPropertyChanged(nameof(IsEditing));
    }

    public void InitForEdit(CategoryDto dto)
    {
        EditingId = dto.Id;
        Name = dto.Name;
        SelectedType = dto.Type;
        SelectedColor = dto.Color;
        ErrorMessage = string.Empty;
        SaveSucceeded = false;
        OnPropertyChanged(nameof(PopupTitle));
        OnPropertyChanged(nameof(IsEditing));
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Name))
        {
            ErrorMessage = "Category name cannot be empty.";
            return;
        }

        var nameExists = await _categoryService.NameExistsAsync(
            Name.Trim(), _session.CurrentUser!.Id, excludeId: EditingId);

        if (nameExists)
        {
            ErrorMessage = "A category with this name already exists.";
            return;
        }

        IsBusy = true;
        try
        {
            if (IsEditing)
                await _categoryService.UpdateAsync(EditingId!.Value, Name.Trim(), SelectedColor);
            else
                await _categoryService.AddAsync(Name.Trim(), SelectedColor, SelectedType, _session.CurrentUser!.Id);

            SaveSucceeded = true;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
