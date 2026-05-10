using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Project.Models.Entities;
using Project.Models.Services;
using Project.Models.Services.Interfaces;
using Project.Views.Popups;

namespace Project.ViewModels;

public partial class CategoryViewModel : BaseViewModel
{
    private readonly ICategoryService _categoryService;
    private readonly SessionService _session;

    public CategoryViewModel(ICategoryService categoryService, SessionService session)
    {
        _categoryService = categoryService;
        _session = session;
    }

    [ObservableProperty]
    public partial ObservableCollection<CategoryDto> IncomeCategories { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<CategoryDto> ExpenseCategories { get; set; } = [];

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        var all = await _categoryService.GetAllAsync(_session.CurrentUser!.Id);
        IncomeCategories  = new ObservableCollection<CategoryDto>(all.Where(c => c.Type == DAL.Enums.TransactionType.Income));
        ExpenseCategories = new ObservableCollection<CategoryDto>(all.Where(c => c.Type == DAL.Enums.TransactionType.Expense));

        IsBusy = false;
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        var vm = IPlatformApplication.Current!.Services.GetRequiredService<AddEditCategoryViewModel>();
        vm.InitForAdd();

        var popup = new AddEditCategoryPopup(vm);
        await Shell.Current.ShowPopupAsync(popup);

        if (vm.SaveSucceeded)
            await LoadAsync();
    }

    [RelayCommand]
    private async Task EditAsync(CategoryDto dto)
    {
        if (dto.IsGlobal) return;

        var confirmed = await Shell.Current.DisplayAlert(
            "Edit Category",
            "Editing this category will affect all its existing transactions. Continue?",
            "Edit", "Cancel");

        if (!confirmed) return;

        var vm = IPlatformApplication.Current!.Services.GetRequiredService<AddEditCategoryViewModel>();
        vm.InitForEdit(dto);

        var popup = new AddEditCategoryPopup(vm);
        await Shell.Current.ShowPopupAsync(popup);

        if (vm.SaveSucceeded)
            await LoadAsync();
    }

    [RelayCommand]
    private async Task DeleteAsync(CategoryDto dto)
    {
        if (dto.IsGlobal) return;

        var hasTransactions = await _categoryService.HasTransactionsAsync(dto.Id);
        if (hasTransactions)
        {
            await Shell.Current.DisplayAlert(
                "Cannot Delete",
                "This category has existing transactions and cannot be deleted.",
                "OK");
            return;
        }

        var confirmed = await Shell.Current.DisplayAlert(
            "Delete Category",
            $"Delete \"{dto.Name}\"? This cannot be undone.",
            "Delete", "Cancel");

        if (!confirmed) return;

        await _categoryService.DeleteAsync(dto.Id);
        await LoadAsync();
    }
}
