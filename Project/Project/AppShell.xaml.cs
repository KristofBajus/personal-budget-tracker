using DAL.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Project.Models.Services;
using Project.Models.Services.Interfaces;
using Project.Views;

namespace Project;

public partial class AppShell : Shell
{
    private readonly IAuthService _authService;
    private readonly SessionService _session;

    public AppShell(IAuthService authService, SessionService session)
    {
        _authService = authService;
        _session = session;
        BindingContext = this;
        InitializeComponent();

        Application.Current!.RequestedThemeChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(IsDarkTheme));
            OnPropertyChanged(nameof(ThemeLabel));
        };
    }

    public bool IsAdmin => _session.CurrentUser?.Role == Role.Admin;
    public bool IsNotAdmin => !IsAdmin;
    public string CurrentUsername => _session.CurrentUser?.Username ?? string.Empty;

    public bool IsDarkTheme
    {
        get
        {
            var app = Application.Current!;
            var effective = app.UserAppTheme != AppTheme.Unspecified
                ? app.UserAppTheme
                : app.RequestedTheme;
            return effective == AppTheme.Dark;
        }
    }

    public string ThemeLabel => IsDarkTheme ? "Dark" : "Light";

    private void OnToggleThemeClicked(object? sender, System.EventArgs e)
    {
        Application.Current!.UserAppTheme = IsDarkTheme ? AppTheme.Light : AppTheme.Dark;
        OnPropertyChanged(nameof(IsDarkTheme));
        OnPropertyChanged(nameof(ThemeLabel));
    }

    private void OnLogoutClicked(object? sender, System.EventArgs e)
    {
        _authService.Logout();
        Application.Current!.MainPage = IPlatformApplication.Current!.Services.GetRequiredService<LoginPage>();
    }
}
