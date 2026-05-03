using Microsoft.Maui.Controls;
using Project.ViewModels;

namespace Project.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
