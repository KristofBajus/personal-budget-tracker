using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Project.Models.Entities;
using Project.Models.Services.Interfaces;

namespace Project.Models.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _db.Users
            .IgnoreQueryFilters()
            .Where(u => u.Role != Role.Admin)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Currency = u.Currency,
                Role = u.Role,
                IsBanned = u.IsBanned,
                IsDeleted = u.IsDeleted,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();
    }

    public async Task BanAsync(int userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return;
        user.IsBanned = true;
        await _db.SaveChangesAsync();
    }

    public async Task UnbanAsync(int userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return;
        user.IsBanned = false;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await _db.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return;
        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task<SystemStatsDto> GetSystemStatsAsync()
    {
        var now = DateTime.UtcNow;
        var users = await _db.Users
            .IgnoreQueryFilters()
            .Where(u => u.Role != Role.Admin)
            .ToListAsync();

        return new SystemStatsDto
        {
            TotalUsers = users.Count,
            ActiveUsers = users.Count(u => !u.IsBanned && !u.IsDeleted),
            BannedUsers = users.Count(u => u.IsBanned && !u.IsDeleted),
            DeletedUsers = users.Count(u => u.IsDeleted),
            TotalTransactions = await _db.Transactions.IgnoreQueryFilters().CountAsync(),
            NewUsersThisMonth = users.Count(u => !u.IsDeleted && u.CreatedAt.Year == now.Year && u.CreatedAt.Month == now.Month)
        };
    }
}
