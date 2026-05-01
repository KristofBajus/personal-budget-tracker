using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL;
using Microsoft.EntityFrameworkCore;
using Project.Models.Entities;
using Project.Models.Services;
using System.Collections.ObjectModel;

namespace Project.ViewModels
{
    public partial class CategoryViewModel : ObservableObject
    {
        private CategoryService _categoryService = new();

        [RelayCommand]
        private async Task Load(object obj)
        {
            Categories = new ObservableCollection<CategoryDto>(await _categoryService.GetCategoriesAsync());
        }

        [ObservableProperty]
        public partial ObservableCollection<CategoryDto> Categories { get; set; } = [];

        public CategoryViewModel()
        {
            using var db = new AppDbContext();
            db.Database.Migrate();
        }
    }
}
    