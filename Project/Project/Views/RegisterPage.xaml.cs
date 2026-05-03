using Microsoft.Maui.Controls;
using Project.ViewModels;

namespace Project.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
