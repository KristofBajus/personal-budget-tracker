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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Application.Current!.RequestedThemeChanged += OnOsThemeChanged;
        UpdateThemeIcons();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Application.Current!.RequestedThemeChanged -= OnOsThemeChanged;
    }

    private void OnOsThemeChanged(object? sender, AppThemeChangedEventArgs e) => UpdateThemeIcons();

    private bool GetIsDarkTheme()
    {
        var app = Application.Current!;
        var effective = app.UserAppTheme != AppTheme.Unspecified ? app.UserAppTheme : app.RequestedTheme;
        return effective == AppTheme.Dark;
    }

    private void UpdateThemeIcons()
    {
        ThemeLightIcon.IsVisible = !GetIsDarkTheme();
        ThemeDarkIcon.IsVisible = GetIsDarkTheme();
    }

    private void OnToggleThemeClicked(object sender, EventArgs e)
    {
        Application.Current!.UserAppTheme = GetIsDarkTheme() ? AppTheme.Light : AppTheme.Dark;
        UpdateThemeIcons();
    }
}
