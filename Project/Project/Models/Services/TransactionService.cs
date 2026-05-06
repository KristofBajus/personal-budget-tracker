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

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _db;

    public TransactionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<decimal> GetBalanceAsync(int userId)
    {
        var income = await _db.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Income)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;
        var expense = await _db.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Expense)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;
        return income - expense;
    }

    public async Task<List<TransactionDto>> GetRecentAsync(int userId, int count = 10)
    {
        return await _db.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .Take(count)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Date = t.Date,
                Type = t.Type,
                Note = t.Note,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                CategoryColor = t.Category.Color,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<decimal> GetMonthlyTotalAsync(int userId, TransactionType type, int year, int month)
    {
        return await _db.Transactions
            .Where(t => t.UserId == userId
                     && t.Type == type
                     && t.Date.Year == year
                     && t.Date.Month == month)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;
    }

    public async Task<List<TransactionDto>> GetFilteredAsync(int userId, TransactionType? type = null, int? categoryId = null, int? month = null, int? year = null)
    {
        var query = _db.Transactions.Where(t => t.UserId == userId);

        if (type is not null)
            query = query.Where(t => t.Type == type);

        if (categoryId is not null)
            query = query.Where(t => t.CategoryId == categoryId);

        if (year is not null)
            query = query.Where(t => t.Date.Year == year);

        if (month is not null)
            query = query.Where(t => t.Date.Month == month);

        return await query
            .OrderByDescending(t => t.Date)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Date = t.Date,
                Type = t.Type,
                Note = t.Note,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                CategoryColor = t.Category.Color,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<TransactionDto> AddAsync(int userId, decimal amount, DateTime date, TransactionType type, int categoryId, string? note)
    {
        var transaction = new DAL.Entities.Transaction
        {
            UserId = userId,
            Amount = amount,
            Date = date,
            Type = type,
            CategoryId = categoryId,
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        var category = await _db.Categories.FindAsync(categoryId);
        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Type = transaction.Type,
            Note = transaction.Note,
            CategoryId = categoryId,
            CategoryName = category!.Name,
            CategoryColor = category.Color,
            CreatedAt = transaction.CreatedAt
        };
    }

    public async Task UpdateAsync(int id, decimal amount, DateTime date, TransactionType type, int categoryId, string? note)
    {
        var transaction = await _db.Transactions.FindAsync(id);
        if (transaction is null) return;

        transaction.Amount = amount;
        transaction.Date = date;
        transaction.Type = type;
        transaction.CategoryId = categoryId;
        transaction.Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var transaction = await _db.Transactions.FindAsync(id);
        if (transaction is null) return;

        transaction.IsDeleted = true;
        transaction.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}
