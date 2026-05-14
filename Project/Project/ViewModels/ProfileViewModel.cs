using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.Models.Services;
using Project.Models.Services.Interfaces;

namespace Project.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly IProfileService _profileService;
    private readonly SessionService _session;

    // Display info
    [ObservableProperty] public partial string Username { get; set; } = string.Empty;
    [ObservableProperty] public partial string Email { get; set; } = string.Empty;
    [ObservableProperty] public partial string Currency { get; set; } = string.Empty;
    [ObservableProperty] public partial DateTime JoinedAt { get; set; }

    // Change username
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasUsernameError))]
    [NotifyPropertyChangedFor(nameof(HasUsernameSuccess))]
    public partial string UsernameError { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasUsernameSuccess))]
    [NotifyPropertyChangedFor(nameof(HasUsernameError))]
    public partial string UsernameSuccess { get; set; } = string.Empty;

    public bool HasUsernameError => !string.IsNullOrEmpty(UsernameError);
    public bool HasUsernameSuccess => !string.IsNullOrEmpty(UsernameSuccess);

    [ObservableProperty] public partial string NewUsername { get; set; } = string.Empty;

    // Change email
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasEmailError))]
    [NotifyPropertyChangedFor(nameof(HasEmailSuccess))]
    public partial string EmailError { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasEmailError))]
    [NotifyPropertyChangedFor(nameof(HasEmailSuccess))]
    public partial string EmailSuccess { get; set; } = string.Empty;

    public bool HasEmailError => !string.IsNullOrEmpty(EmailError);
    public bool HasEmailSuccess => !string.IsNullOrEmpty(EmailSuccess);

    [ObservableProperty] public partial string NewEmail { get; set; } = string.Empty;

    // Change password
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPasswordError))]
    [NotifyPropertyChangedFor(nameof(HasPasswordSuccess))]
    public partial string PasswordError { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPasswordError))]
    [NotifyPropertyChangedFor(nameof(HasPasswordSuccess))]
    public partial string PasswordSuccess { get; set; } = string.Empty;

    public bool HasPasswordError => !string.IsNullOrEmpty(PasswordError);
    public bool HasPasswordSuccess => !string.IsNullOrEmpty(PasswordSuccess);

    [ObservableProperty] public partial bool IsCurrentPasswordVisible { get; set; } = false;
    [ObservableProperty] public partial bool IsNewPasswordVisible { get; set; } = false;
    [ObservableProperty] public partial bool IsConfirmPasswordVisible { get; set; } = false;

    [RelayCommand] private void ToggleCurrentPasswordVisibility() => IsCurrentPasswordVisible = !IsCurrentPasswordVisible;
    [RelayCommand] private void ToggleNewPasswordVisibility() => IsNewPasswordVisible = !IsNewPasswordVisible;
    [RelayCommand] private void ToggleConfirmPasswordVisibility() => IsConfirmPasswordVisible = !IsConfirmPasswordVisible;

    [ObservableProperty] public partial string CurrentPassword { get; set; } = string.Empty;
    [ObservableProperty] public partial string NewPassword { get; set; } = string.Empty;
    [ObservableProperty] public partial string ConfirmPassword { get; set; } = string.Empty;

    partial void OnNewUsernameChanged(string value) { UsernameError = string.Empty; UsernameSuccess = string.Empty; }
    partial void OnNewEmailChanged(string value) { EmailError = string.Empty; EmailSuccess = string.Empty; }
    partial void OnCurrentPasswordChanged(string value) { PasswordError = string.Empty; PasswordSuccess = string.Empty; }
    partial void OnNewPasswordChanged(string value) { PasswordError = string.Empty; PasswordSuccess = string.Empty; }
    partial void OnConfirmPasswordChanged(string value) { PasswordError = string.Empty; PasswordSuccess = string.Empty; }

    public ProfileViewModel(IProfileService profileService, SessionService session)
    {
        _profileService = profileService;
        _session = session;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (_session.CurrentUser is null) return;
        var profile = await _profileService.GetProfileAsync(_session.CurrentUser.Id);
        Username = profile.Username;
        Email = profile.Email;
        Currency = profile.Currency;
        JoinedAt = profile.CreatedAt;
    }

    [RelayCommand]
    private async Task UpdateUsernameAsync()
    {
        UsernameError = string.Empty;
        UsernameSuccess = string.Empty;

        if (string.IsNullOrWhiteSpace(NewUsername))
        {
            UsernameError = "Username cannot be empty.";
            return;
        }

        try
        {
            IsBusy = true;
            await _profileService.UpdateUsernameAsync(_session.CurrentUser!.Id, NewUsername);
            _session.SetUser(await _profileService.GetProfileAsync(_session.CurrentUser.Id));
            Username = _session.CurrentUser.Username;
            NewUsername = string.Empty;
            UsernameSuccess = "Username updated successfully.";
        }
        catch (InvalidOperationException ex)
        {
            UsernameError = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task UpdateEmailAsync()
    {
        EmailError = string.Empty;
        EmailSuccess = string.Empty;

        if (string.IsNullOrWhiteSpace(NewEmail))
        {
            EmailError = "Email cannot be empty.";
            return;
        }

        if (!Regex.IsMatch(NewEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            EmailError = "Please enter a valid email address.";
            return;
        }

        try
        {
            IsBusy = true;
            await _profileService.UpdateEmailAsync(_session.CurrentUser!.Id, NewEmail);
            _session.SetUser(await _profileService.GetProfileAsync(_session.CurrentUser.Id));
            Email = _session.CurrentUser.Email;
            NewEmail = string.Empty;
            EmailSuccess = "Email updated successfully.";
        }
        catch (InvalidOperationException ex)
        {
            EmailError = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        PasswordError = string.Empty;
        PasswordSuccess = string.Empty;

        if (string.IsNullOrWhiteSpace(CurrentPassword) ||
            string.IsNullOrWhiteSpace(NewPassword) ||
            string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            PasswordError = "All password fields are required.";
            return;
        }

        var passwordError = ValidatePassword(NewPassword);
        if (passwordError is not null)
        {
            PasswordError = passwordError;
            return;
        }

        if (NewPassword != ConfirmPassword)
        {
            PasswordError = "New passwords do not match.";
            return;
        }

        try
        {
            IsBusy = true;
            await _profileService.ChangePasswordAsync(_session.CurrentUser!.Id, CurrentPassword, NewPassword);
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
            PasswordSuccess = "Password changed successfully.";
        }
        catch (InvalidOperationException ex)
        {
            PasswordError = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

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
