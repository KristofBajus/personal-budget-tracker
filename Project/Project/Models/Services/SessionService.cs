using Project.Models.Entities;

namespace Project.Models.Services;

public class SessionService
{
    public UserDto? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;

    public void SetUser(UserDto user) => CurrentUser = user;
    public void Clear() => CurrentUser = null;
}
