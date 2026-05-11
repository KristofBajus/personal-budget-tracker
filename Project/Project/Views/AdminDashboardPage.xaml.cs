using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using Project.ViewModels;

namespace Project.Views;

public partial class AdminDashboardPage : ContentPage
{
    public AdminDashboardPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current!.Services.GetRequiredService<AdminDashboardViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((AdminDashboardViewModel)BindingContext).LoadCommand.ExecuteAsync(null);
    }
}
