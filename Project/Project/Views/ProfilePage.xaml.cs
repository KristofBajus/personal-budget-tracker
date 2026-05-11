using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Project.ViewModels;

namespace Project.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current!.Services.GetRequiredService<ProfileViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ProfileViewModel)BindingContext).LoadCommand.ExecuteAsync(null);
    }
}
