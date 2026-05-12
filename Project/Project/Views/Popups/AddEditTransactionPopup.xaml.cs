using System;
using CommunityToolkit.Maui.Views;
using Project.ViewModels;

namespace Project.Views.Popups;

public partial class AddEditTransactionPopup : Popup
{
    public AddEditTransactionPopup(AddEditTransactionViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        vm.PropertyChanged += async (s, e) =>
        {
            if (e.PropertyName == nameof(AddEditTransactionViewModel.SaveSucceeded) && vm.SaveSucceeded)
                try { await CloseAsync(); } catch { /* popup already closed */ }
        };
    }

    private async void OnCancelClicked(object? sender, EventArgs e) => await CloseAsync();
}
