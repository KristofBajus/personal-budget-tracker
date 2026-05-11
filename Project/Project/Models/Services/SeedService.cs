using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using DAL;
using DAL.Entities;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Project.Models.Services;

public static class SeedService
{
    public static async Task SeedAdminAsync(AppDbContext db)
    {
        var adminExists = await db.Users
            .IgnoreQueryFilters()
            .AnyAsync(u => u.Role == Role.Admin);

        if (adminExists) return;

        db.Users.Add(new User
        {
            Username = "admin",
            Email = "admin@budgettracker.local",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234"),
            Role = Role.Admin,
            Currency = "EUR",
            CreatedAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }
}
