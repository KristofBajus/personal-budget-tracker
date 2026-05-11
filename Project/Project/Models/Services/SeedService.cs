using System;
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

    public static async Task SeedDemoDataAsync(AppDbContext db)
    {
        var demoExists = await db.Users
            .IgnoreQueryFilters()
            .AnyAsync(u => u.Username == "demo");

        if (demoExists) return;

        // ── Users ──────────────────────────────────────────────────────────
        var demo = new User
        {
            Username = "demo",
            Email = "demo@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Demo1234"),
            Role = Role.User,
            Currency = "EUR",
            CreatedAt = D(2026, 1, 10)
        };
        var student = new User
        {
            Username = "student",
            Email = "student@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student1234"),
            Role = Role.User,
            Currency = "CZK",
            CreatedAt = D(2026, 2, 1)
        };
        var empty = new User
        {
            Username = "empty",
            Email = "empty@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Empty1234"),
            Role = Role.User,
            Currency = "USD",
            CreatedAt = D(2026, 4, 20)
        };
        db.Users.AddRange(demo, student, empty);
        await db.SaveChangesAsync();

        // ── Custom categories ──────────────────────────────────────────────
        var consulting = new Category { Name = "Consulting",    Color = "#00BCD4", Type = TransactionType.Income,  IsGlobal = false, UserId = demo.Id,    CreatedAt = D(2026, 1, 10) };
        var gym        = new Category { Name = "Gym",           Color = "#FF5722", Type = TransactionType.Expense, IsGlobal = false, UserId = demo.Id,    CreatedAt = D(2026, 1, 10) };
        var partTime   = new Category { Name = "Part-time Job", Color = "#66BB6A", Type = TransactionType.Income,  IsGlobal = false, UserId = student.Id, CreatedAt = D(2026, 2, 1)  };
        db.Categories.AddRange(consulting, gym, partTime);
        await db.SaveChangesAsync();

        // Global category IDs from AppDbContext seed
        const int salaryId        = 1;
        const int allowanceId     = 4;
        const int foodId          = 6;
        const int housingId       = 7;
        const int transportId     = 8;
        const int healthId        = 9;
        const int entertainmentId = 10;
        const int shoppingId      = 11;

        // ── demo user transactions (EUR) ───────────────────────────────────
        db.Transactions.AddRange(
            // February 2026
            Tx(demo.Id, salaryId,        TransactionType.Income,  2000m, "February salary",      D(2026, 2, 1)),
            Tx(demo.Id, housingId,       TransactionType.Expense,  800m, "Rent",                  D(2026, 2, 1)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   45m, "Groceries",             D(2026, 2, 5)),
            Tx(demo.Id, transportId,     TransactionType.Expense,   25m, "Metro pass",            D(2026, 2, 8)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   38m, "Groceries",             D(2026, 2, 10)),
            Tx(demo.Id, entertainmentId, TransactionType.Expense,   30m, "Valentine's dinner",    D(2026, 2, 14)),
            Tx(demo.Id, gym.Id,          TransactionType.Expense,   40m, "Monthly membership",    D(2026, 2, 15)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   52m, "Groceries",             D(2026, 2, 18)),
            Tx(demo.Id, transportId,     TransactionType.Expense,   25m, "Fuel",                  D(2026, 2, 22)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   45m, "Groceries",             D(2026, 2, 25)),
            Tx(demo.Id, entertainmentId, TransactionType.Expense,   20m, "Streaming services",    D(2026, 2, 28)),
            // March 2026
            Tx(demo.Id, salaryId,        TransactionType.Income,  2000m, "March salary",          D(2026, 3, 1)),
            Tx(demo.Id, housingId,       TransactionType.Expense,  800m, "Rent",                  D(2026, 3, 1)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   48m, "Groceries",             D(2026, 3, 5)),
            Tx(demo.Id, consulting.Id,   TransactionType.Income,   500m, "Website project",       D(2026, 3, 8)),
            Tx(demo.Id, transportId,     TransactionType.Expense,   30m, "Metro pass",            D(2026, 3, 10)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   42m, "Groceries",             D(2026, 3, 12)),
            Tx(demo.Id, gym.Id,          TransactionType.Expense,   40m, "Monthly membership",    D(2026, 3, 15)),
            Tx(demo.Id, shoppingId,      TransactionType.Expense,   85m, "Clothes",               D(2026, 3, 18)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   55m, "Groceries",             D(2026, 3, 20)),
            Tx(demo.Id, transportId,     TransactionType.Expense,   25m, "Fuel",                  D(2026, 3, 22)),
            Tx(demo.Id, entertainmentId, TransactionType.Expense,   45m, "Concert tickets",       D(2026, 3, 26)),
            Tx(demo.Id, healthId,        TransactionType.Expense,   60m, "Doctor visit",          D(2026, 3, 28)),
            // April 2026
            Tx(demo.Id, salaryId,        TransactionType.Income,  2000m, "April salary",          D(2026, 4, 1)),
            Tx(demo.Id, housingId,       TransactionType.Expense,  800m, "Rent",                  D(2026, 4, 1)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   50m, "Groceries",             D(2026, 4, 4)),
            Tx(demo.Id, transportId,     TransactionType.Expense,   28m, "Metro pass",            D(2026, 4, 7)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   44m, "Groceries",             D(2026, 4, 10)),
            Tx(demo.Id, entertainmentId, TransactionType.Expense,   35m, "Cinema",                D(2026, 4, 13)),
            Tx(demo.Id, gym.Id,          TransactionType.Expense,   40m, "Monthly membership",    D(2026, 4, 15)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   58m, "Groceries",             D(2026, 4, 18)),
            Tx(demo.Id, shoppingId,      TransactionType.Expense,  120m, "New headphones",        D(2026, 4, 22)),
            Tx(demo.Id, transportId,     TransactionType.Expense,   25m, "Fuel",                  D(2026, 4, 25)),
            Tx(demo.Id, foodId,          TransactionType.Expense,   42m, "Groceries",             D(2026, 4, 27)),
            Tx(demo.Id, entertainmentId, TransactionType.Expense,   25m, "Streaming services",    D(2026, 4, 30))
        );

        // ── student user transactions (CZK) ───────────────────────────────
        db.Transactions.AddRange(
            // March 2026
            Tx(student.Id, allowanceId,     TransactionType.Income,  5000m, "Monthly allowance",  D(2026, 3, 1)),
            Tx(student.Id, housingId,       TransactionType.Expense, 3000m, "Rent",               D(2026, 3, 1)),
            Tx(student.Id, foodId,          TransactionType.Expense,  300m, "Groceries",          D(2026, 3, 5)),
            Tx(student.Id, transportId,     TransactionType.Expense,  200m, "Bus pass",           D(2026, 3, 10)),
            Tx(student.Id, partTime.Id,     TransactionType.Income,  3000m, "Part-time job",      D(2026, 3, 12)),
            Tx(student.Id, foodId,          TransactionType.Expense,  250m, "Groceries",          D(2026, 3, 15)),
            Tx(student.Id, entertainmentId, TransactionType.Expense,  150m, "Games and cinema",   D(2026, 3, 18)),
            Tx(student.Id, foodId,          TransactionType.Expense,  280m, "Groceries",          D(2026, 3, 22)),
            Tx(student.Id, foodId,          TransactionType.Expense,  200m, "Groceries",          D(2026, 3, 28)),
            // April 2026
            Tx(student.Id, allowanceId,     TransactionType.Income,  5000m, "Monthly allowance",  D(2026, 4, 1)),
            Tx(student.Id, housingId,       TransactionType.Expense, 3000m, "Rent",               D(2026, 4, 1)),
            Tx(student.Id, foodId,          TransactionType.Expense,  320m, "Groceries",          D(2026, 4, 5)),
            Tx(student.Id, transportId,     TransactionType.Expense,  200m, "Bus pass",           D(2026, 4, 8)),
            Tx(student.Id, partTime.Id,     TransactionType.Income,  3000m, "Part-time job",      D(2026, 4, 12)),
            Tx(student.Id, foodId,          TransactionType.Expense,  270m, "Groceries",          D(2026, 4, 15)),
            Tx(student.Id, entertainmentId, TransactionType.Expense,  200m, "Games",              D(2026, 4, 20)),
            Tx(student.Id, shoppingId,      TransactionType.Expense,  500m, "Textbooks",          D(2026, 4, 24)),
            Tx(student.Id, foodId,          TransactionType.Expense,  250m, "Groceries",          D(2026, 4, 28))
        );

        await db.SaveChangesAsync();
    }

    private static Transaction Tx(int userId, int categoryId, TransactionType type, decimal amount, string note, DateTime date) =>
        new Transaction
        {
            UserId = userId,
            CategoryId = categoryId,
            Type = type,
            Amount = amount,
            Note = note,
            Date = date,
            CreatedAt = date
        };

    private static DateTime D(int year, int month, int day) =>
        new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
}
