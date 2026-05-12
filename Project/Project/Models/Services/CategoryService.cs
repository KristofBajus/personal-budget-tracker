using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Project.Models.Entities;
using Project.Models.Services.Interfaces;

namespace Project.Models.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CategoryDto>> GetAllAsync(int userId)
    {
        // returning all global plus users categories
        return await _db.Categories
            .Where(c => c.IsGlobal || c.UserId == userId)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Color = c.Color,
                Type = c.Type,
                IsGlobal = c.IsGlobal,
                UserId = c.UserId
            })
            .ToListAsync();
    }

    public async Task<CategoryDto> AddAsync(string name, string color, TransactionType type, int userId)
    {
        var category = new Category
        {
            Name = name,
            Color = color,
            Type = type,
            UserId = userId,
            IsGlobal = false,
            CreatedAt = DateTime.UtcNow
        };
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return new CategoryDto { Id = category.Id, Name = name, Color = color, Type = type, IsGlobal = false, UserId = userId };
    }

    public async Task UpdateAsync(int id, string name, string color)
    {
        var category = await _db.Categories.FindAsync(id)
            ?? throw new InvalidOperationException("Category not found.");
        category.Name = name;
        category.Color = color;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id)
            ?? throw new InvalidOperationException("Category not found.");
        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task<bool> HasTransactionsAsync(int id)
    {
        return await _db.Transactions.AnyAsync(t => t.CategoryId == id);
    }

    public async Task<bool> NameExistsAsync(string name, int userId, int? excludeId = null)
    {
        return await _db.Categories
            .Where(c => (c.IsGlobal || c.UserId == userId)
                     && c.Name.ToLower() == name.ToLower()
                     && (excludeId == null || c.Id != excludeId))
            .AnyAsync();
    }
}
