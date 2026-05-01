using DAL.Entities;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            options.UseSqlite($"Data Source={Path.Combine(folder, "budget.db")}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Automatically appends WHERE IsDeleted = 0 to every query — soft-deleted records are invisible by default
        // Use .IgnoreQueryFilters() on a specific query to bypass this (e.g. Admin listing deleted users)
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Transaction>().HasQueryFilter(t => !t.IsDeleted);
        

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        var seededAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Category>().HasData(
            // Income
            new Category { Id = 1, Name = "Salary",        Color = "#4CAF50", Type = TransactionType.Income,  IsGlobal = true, CreatedAt = seededAt },
            new Category { Id = 2, Name = "Freelance",     Color = "#8BC34A", Type = TransactionType.Income,  IsGlobal = true, CreatedAt = seededAt },
            new Category { Id = 3, Name = "Gift",          Color = "#FF9800", Type = TransactionType.Income,  IsGlobal = true, CreatedAt = seededAt },
            new Category { Id = 4, Name = "Allowance",     Color = "#FFC107", Type = TransactionType.Income,  IsGlobal = true, CreatedAt = seededAt },
            new Category { Id = 5, Name = "Other",         Color = "#9E9E9E", Type = TransactionType.Income,  IsGlobal = true, CreatedAt = seededAt },
            // Expense
            new Category { Id = 6,  Name = "Food",          Color = "#FF6B6B", Type = TransactionType.Expense, IsGlobal = true, CreatedAt = seededAt },
            new Category { Id = 7,  Name = "Housing",       Color = "#9C27B0", Type = TransactionType.Expense, IsGlobal = true, CreatedAt = seededAt },
            new Category { Id = 8,  Name = "Transport",     Color = "#4ECDC4", Type = TransactionType.Expense, IsGlobal = true, CreatedAt = seededAt },
            new Category { Id = 9,  Name = "Health",        Color = "#F44336", Type = TransactionType.Expense, IsGlobal = true, CreatedAt = seededAt },
            new Category { Id = 10, Name = "Entertainment", Color = "#45B7D1", Type = TransactionType.Expense, IsGlobal = true, CreatedAt = seededAt },
            new Category { Id = 11, Name = "Shopping",      Color = "#E91E63", Type = TransactionType.Expense, IsGlobal = true, CreatedAt = seededAt },
            new Category { Id = 12, Name = "Other",         Color = "#607D8B", Type = TransactionType.Expense, IsGlobal = true, CreatedAt = seededAt }
        );
    }
}
