using Microsoft.Maui.Controls;
using Project.ViewModels;

namespace Project.Views;

public partial class CategoryView : ContentPage
{
    // Parameterless constructor used by Shell's DataTemplate (Activator.CreateInstance)
    public CategoryView()
    {
        InitializeComponent();
    }

    // Constructor used when resolved via DI (Phase 7 onwards)
    public CategoryView(CategoryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
