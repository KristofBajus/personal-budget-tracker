using Project.Models.Entities;

namespace Project.Models.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(string username, string email, string password, string currency);
    Task<UserDto?> LoginAsync(string username, string password);
    void Logout();
}
