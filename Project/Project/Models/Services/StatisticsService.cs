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

public class StatisticsService : IStatisticsService
{
    private readonly AppDbContext _db;

    public StatisticsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<MonthlyTotalDto>> GetMonthlyTotalsAsync(int userId, DateTime from)
    {
        var rows = await _db.Transactions
            .Where(t => t.UserId == userId && t.Date >= from)
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .Select(g => new MonthlyTotalDto
            {
                Year    = g.Key.Year,
                Month   = g.Key.Month,
                Income  = g.Where(t => t.Type == TransactionType.Income).Sum(t => (decimal?)t.Amount) ?? 0m,
                Expense = g.Where(t => t.Type == TransactionType.Expense).Sum(t => (decimal?)t.Amount) ?? 0m
            })
            .OrderBy(m => m.Year).ThenBy(m => m.Month)
            .ToListAsync();

        return rows;
    }

    public async Task<List<CategoryBreakdownDto>> GetCategoryBreakdownAsync(int userId, DateTime from)
    {
        return await _db.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Expense && t.Date >= from)
            .GroupBy(t => new { t.CategoryId, t.Category.Name, t.Category.Color })
            .Select(g => new CategoryBreakdownDto
            {
                CategoryId = g.Key.CategoryId,
                Name       = g.Key.Name,
                Color      = g.Key.Color,
                Total      = g.Sum(t => (decimal?)t.Amount) ?? 0m
            })
            .OrderByDescending(c => c.Total)
            .ToListAsync();
    }

    public async Task<List<BalancePointDto>> GetBalanceHistoryAsync(int userId, DateTime from)
    {
        var startingBalance = await _db.Transactions
            .Where(t => t.UserId == userId && t.Date < from)
            .SumAsync(t => t.Type == TransactionType.Income
                ? (decimal?)t.Amount
                : -(decimal?)t.Amount) ?? 0m;

        var transactions = await _db.Transactions
            .Where(t => t.UserId == userId && t.Date >= from)
            .OrderBy(t => t.Date)
            .Select(t => new { t.Date, t.Amount, t.Type })
            .ToListAsync();

        var points = new List<BalancePointDto>();
        var running = startingBalance;

        foreach (var tx in transactions)
        {
            running += tx.Type == TransactionType.Income ? tx.Amount : -tx.Amount;
            points.Add(new BalancePointDto { Date = tx.Date, Balance = running });
        }

        return points;
    }
}
