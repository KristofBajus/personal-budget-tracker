using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Project.Models.Services.Interfaces;
using Project.ViewModels;

namespace Project.ViewModels;

public partial class AdminDashboardViewModel : BaseViewModel
{
    private readonly IUserService _userService;

    [ObservableProperty] public partial int TotalUsers { get; set; }
    [ObservableProperty] public partial int ActiveUsers { get; set; }
    [ObservableProperty] public partial int BannedUsers { get; set; }
    [ObservableProperty] public partial int DeletedUsers { get; set; }
    [ObservableProperty] public partial int TotalTransactions { get; set; }
    [ObservableProperty] public partial int NewUsersThisMonth { get; set; }

    public AdminDashboardViewModel(IUserService userService)
    {
        _userService = userService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var stats = await _userService.GetSystemStatsAsync();
            TotalUsers = stats.TotalUsers;
            ActiveUsers = stats.ActiveUsers;
            BannedUsers = stats.BannedUsers;
            DeletedUsers = stats.DeletedUsers;
            TotalTransactions = stats.TotalTransactions;
            NewUsersThisMonth = stats.NewUsersThisMonth;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
