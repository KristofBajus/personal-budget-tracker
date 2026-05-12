using System;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Project.Models.Exceptions;
using Project.Models.Entities;
using Project.Models.Services.Interfaces;

namespace Project.Models.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly SessionService _session;

    public AuthService(AppDbContext db, SessionService session)
    {
        _db = db;
        _session = session;
    }

    public async Task<UserDto> RegisterAsync(string username, string email, string password, string currency)
    {
        // IgnoreQueryFilters so soft-deleted usernames are still considered taken
        var taken = await _db.Users
            .IgnoreQueryFilters()
            .AnyAsync(u => u.Username == username);

        if (taken)
            throw new InvalidOperationException("Username is already taken.");

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password)),
            Currency = currency,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        
        // auto log in after registration
        var dto = ToDto(user);
        _session.SetUser(dto);
        return dto;
    }

    public async Task<UserDto?> LoginAsync(string username, string password)
    {
        // IgnoreQueryFilters so we can distinguish deleted vs banned vs not found
        var user = await _db.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Username == username);

        // Not found or soft-deleted → generic error (don't reveal account existence)
        if (user is null || user.IsDeleted)
            return null;

        // Banned → specific message via exception
        if (user.IsBanned)
            throw new AccountBannedException();

        var valid = await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, user.PasswordHash));
        if (!valid)
            return null;

        var dto = ToDto(user);
        _session.SetUser(dto);
        return dto;
    }

    public void Logout()
    {
        _session.Clear();
    }

    private static UserDto ToDto(User u) => new()
    {
        Id = u.Id,
        Username = u.Username,
        Email = u.Email,
        Currency = u.Currency,
        Role = u.Role,
        IsBanned = u.IsBanned,
        CreatedAt = u.CreatedAt
    };
}
