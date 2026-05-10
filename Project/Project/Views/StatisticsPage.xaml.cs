using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Project.ViewModels;

namespace Project.Views;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current!.Services.GetRequiredService<StatisticsViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((StatisticsViewModel)BindingContext).LoadCommand.ExecuteAsync(null);
    }
}
