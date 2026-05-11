using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Project.Models.Services.Interfaces;
using Project.Views;

namespace Project.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public RegisterViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    public List<string> Currencies { get; } = ["EUR", "USD", "CZK", "GBP", "CHF"];

    [ObservableProperty] public partial bool IsPasswordVisible { get; set; } = false;

    [RelayCommand]
    private void TogglePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    [ObservableProperty]
    public partial string Username { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Password { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ConfirmPassword { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string SelectedCurrency { get; set; } = "EUR";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    [RelayCommand]
    private async Task RegisterAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorMessage = "Username is required.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            ErrorMessage = "Email is required.";
            return;
        }

        if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            ErrorMessage = "Please enter a valid email address.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Password is required.";
            return;
        }

        var passwordError = ValidatePassword(Password);
        if (passwordError is not null)
        {
            ErrorMessage = passwordError;
            return;
        }

        if (string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "Please confirm your password.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }

        IsBusy = true;
        try
        {
            await _authService.RegisterAsync(Username, Email, Password, SelectedCurrency);
            Application.Current!.MainPage = IPlatformApplication.Current!.Services.GetRequiredService<AppShell>();
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void GoToLogin()
    {
        var loginPage = IPlatformApplication.Current!.Services.GetRequiredService<LoginPage>();
        Application.Current!.MainPage = loginPage;
    }

    // Min 8 chars, at least one uppercase, one lowercase, one digit
    private static string? ValidatePassword(string password)
    {
        if (password.Length < 8)
            return "Password must be at least 8 characters.";
        if (!password.Any(char.IsUpper))
            return "Password must contain at least one uppercase letter.";
        if (!password.Any(char.IsLower))
            return "Password must contain at least one lowercase letter.";
        if (!password.Any(char.IsDigit))
            return "Password must contain at least one number.";
        return null;
    }
}
