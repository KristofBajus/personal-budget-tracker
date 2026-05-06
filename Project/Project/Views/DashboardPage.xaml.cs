using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using Project.ViewModels;

namespace Project.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current!.Services.GetRequiredService<DashboardViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((DashboardViewModel)BindingContext).LoadCommand.ExecuteAsync(null);
    }
}
