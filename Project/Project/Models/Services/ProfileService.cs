using System;
using System.Threading.Tasks;
using DAL;
using Microsoft.EntityFrameworkCore;
using Project.Models.Entities;
using Project.Models.Services.Interfaces;

namespace Project.Models.Services;

public class ProfileService : IProfileService
{
    private readonly AppDbContext _db;

    public ProfileService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserDto> GetProfileAsync(int userId)
    {
        var user = await _db.Users.FindAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Currency = user.Currency,
            Role = user.Role,
            IsBanned = user.IsBanned,
            IsDeleted = user.IsDeleted,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task UpdateUsernameAsync(int userId, string username)
    {
        var taken = await _db.Users.AnyAsync(u => u.Username == username && u.Id != userId);
        if (taken) throw new InvalidOperationException("Username is already taken.");

        var user = await _db.Users.FindAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        user.Username = username;
        await _db.SaveChangesAsync();
    }

    public async Task UpdateEmailAsync(int userId, string email)
    {
        var taken = await _db.Users.AnyAsync(u => u.Email == email && u.Id != userId);
        if (taken) throw new InvalidOperationException("Email is already in use.");

        var user = await _db.Users.FindAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        user.Email = email;
        await _db.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await _db.Users.FindAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        var valid = await Task.Run(() => BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash));
        if (!valid)
            throw new InvalidOperationException("Current password is incorrect.");

        user.PasswordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(newPassword));
        await _db.SaveChangesAsync();
    }
}
