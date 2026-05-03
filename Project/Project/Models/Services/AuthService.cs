using Project.Models.Entities;
using Project.Models.Services.Interfaces;

namespace Project.Models.Services;

public class AuthService : IAuthService
{
    public Task<UserDto> RegisterAsync(string username, string email, string password, string currency)
        => throw new NotImplementedException();

    public Task<UserDto?> LoginAsync(string username, string password)
        => throw new NotImplementedException();

    public void Logout()
        => throw new NotImplementedException();
}
