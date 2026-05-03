using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.Models.Entities;
using Project.Models.Services;
using Project.Models.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
    public partial ObservableCollection<CategoryDto> Categories { get; set; } = [];

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        Categories = new ObservableCollection<CategoryDto>(
            await _categoryService.GetAllAsync(_session.CurrentUser!.Id));
        IsBusy = false;
    }
}
