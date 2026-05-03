using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using Project.Models.Exceptions;
using Project.Models.Services.Interfaces;
using Project.Views;

namespace Project.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    public partial string Username { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Password { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorMessage = "Username is required.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Password is required.";
            return;
        }

        IsBusy = true;
        try
        {
            var user = await _authService.LoginAsync(Username, Password);
            if (user is null)
            {
                ErrorMessage = "Invalid username or password.";
                return;
            }

            // Phase 7 will replace this with the proper auth gate navigation
            Application.Current!.MainPage = new AppShell();
        }
        catch (AccountBannedException)
        {
            ErrorMessage = "Your account has been suspended.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void GoToRegister()
    {
        var registerPage = IPlatformApplication.Current!.Services.GetRequiredService<RegisterPage>();
        Application.Current!.MainPage = registerPage;
    }
}
