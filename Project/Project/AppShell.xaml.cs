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
    }

    public bool IsAdmin => _session.CurrentUser?.Role == Role.Admin;
    public string CurrentUsername => _session.CurrentUser?.Username ?? string.Empty;

    private void OnLogoutClicked(object? sender, System.EventArgs e)
    {
        _authService.Logout();
        Application.Current!.MainPage = IPlatformApplication.Current!.Services.GetRequiredService<LoginPage>();
    }
}
