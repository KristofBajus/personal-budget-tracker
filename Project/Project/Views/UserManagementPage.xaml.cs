using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Project.ViewModels;

namespace Project.Views;

public partial class UserManagementPage : ContentPage
{
    public UserManagementPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current!.Services.GetRequiredService<UserManagementViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((UserManagementViewModel)BindingContext).LoadCommand.ExecuteAsync(null);
    }
}
