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

    public Task<List<TransactionDto>> GetFilteredAsync(int userId, TransactionType? type = null, int? categoryId = null, int? month = null, int? year = null)
        => throw new NotImplementedException();

    public Task<TransactionDto> AddAsync(int userId, decimal amount, DateTime date, TransactionType type, int categoryId, string? note)
        => throw new NotImplementedException();

    public Task UpdateAsync(int id, decimal amount, DateTime date, TransactionType type, int categoryId, string? note)
        => throw new NotImplementedException();

    public Task DeleteAsync(int id)
        => throw new NotImplementedException();
}
