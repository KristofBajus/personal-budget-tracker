using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.Models.Entities;
using Project.Models.Services;
using Project.Models.Services.Interfaces;

namespace Project.ViewModels;

public partial class UserManagementViewModel : BaseViewModel
{
    private readonly IUserService _userService;
    private readonly SessionService _session;

    private List<UserDto> _allUsers = [];

    public ObservableCollection<UserDto> Users { get; } = [];

    public List<string> SortOptions { get; } = ["Newest First", "Oldest First"];
    public List<string> StatusOptions { get; } = ["All", "Active", "Banned", "Deleted"];

    [ObservableProperty] 
    public partial string SearchText { get; set; } = string.Empty;
    [ObservableProperty] 
    public partial string SelectedSort { get; set; } = "Newest First";
    [ObservableProperty] 
    public partial string SelectedStatus { get; set; } = "All";

    public UserManagementViewModel(IUserService userService, SessionService session)
    {
        _userService = userService;
        _session = session;
    }

    partial void OnSearchTextChanged(string value) => ApplyFilters();
    partial void OnSelectedSortChanged(string value) => ApplyFilters();
    partial void OnSelectedStatusChanged(string value) => ApplyFilters();

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            _allUsers = await _userService.GetAllAsync();
            ApplyFilters();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task BanAsync(UserDto user)
    {
        if (user.Id == _session.CurrentUser?.Id) return;
        var confirmed = await Shell.Current.DisplayAlert(
            "Ban User",
            $"Are you sure you want to ban @{user.Username}?",
            "Ban", "Cancel");
        if (!confirmed) return;
        await _userService.BanAsync(user.Id);
        await ReloadAsync();
    }

    [RelayCommand]
    private async Task UnbanAsync(UserDto user)
    {
        var confirmed = await Shell.Current.DisplayAlert(
            "Unban User",
            $"Unban @{user.Username}?",
            "Unban", "Cancel");
        if (!confirmed) return;
        await _userService.UnbanAsync(user.Id);
        await ReloadAsync();
    }

    [RelayCommand]
    private async Task DeleteAsync(UserDto user)
    {
        if (user.Id == _session.CurrentUser?.Id) return;
        var confirmed = await Shell.Current.DisplayAlert(
            "Delete User",
            $"This will permanently remove @{user.Username}. This cannot be undone.",
            "Delete", "Cancel");
        if (!confirmed) return;
        await _userService.DeleteAsync(user.Id);
        await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        IsBusy = true;
        try
        {
            _allUsers = await _userService.GetAllAsync();
            ApplyFilters();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ApplyFilters()
    {
        var filtered = _allUsers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(u =>
                u.Username.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase));

        filtered = SelectedStatus switch
        {
            "Active"  => filtered.Where(u => !u.IsBanned && !u.IsDeleted),
            "Banned"  => filtered.Where(u => u.IsBanned && !u.IsDeleted),
            "Deleted" => filtered.Where(u => u.IsDeleted),
            _         => filtered
        };

        filtered = SelectedSort == "Oldest First"
            ? filtered.OrderBy(u => u.CreatedAt)
            : filtered.OrderByDescending(u => u.CreatedAt);

        Users.Clear();
        foreach (var u in filtered)
            Users.Add(u);
    }
}
