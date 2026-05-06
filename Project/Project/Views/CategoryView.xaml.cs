using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Project.ViewModels;

namespace Project.Views;

public partial class CategoryView : ContentPage
{
    public CategoryView()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current!.Services.GetRequiredService<CategoryViewModel>();
    }
}
