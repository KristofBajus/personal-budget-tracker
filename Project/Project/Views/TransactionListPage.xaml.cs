using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Project.ViewModels;

namespace Project.Views;

public partial class TransactionListPage : ContentPage
{
    public TransactionListPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current!.Services.GetRequiredService<TransactionListViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((TransactionListViewModel)BindingContext).LoadCommand.ExecuteAsync(null);
    }
}
