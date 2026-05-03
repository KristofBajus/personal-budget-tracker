using System;
using System.Collections.Generic;
using DAL.Enums;
using Project.Models.Entities;
using System.Threading.Tasks;
using Project.Models.Services.Interfaces;

namespace Project.Models.Services;

public class TransactionService : ITransactionService
{
    public Task<List<TransactionDto>> GetFilteredAsync(int userId, TransactionType? type = null, int? categoryId = null, int? month = null, int? year = null)
        => throw new NotImplementedException();

    public Task<List<TransactionDto>> GetRecentAsync(int userId, int count = 10)
        => throw new NotImplementedException();

    public Task<decimal> GetBalanceAsync(int userId)
        => throw new NotImplementedException();

    public Task<decimal> GetMonthlyTotalAsync(int userId, TransactionType type, int year, int month)
        => throw new NotImplementedException();

    public Task<TransactionDto> AddAsync(int userId, decimal amount, DateTime date, TransactionType type, int categoryId, string? note)
        => throw new NotImplementedException();

    public Task UpdateAsync(int id, decimal amount, DateTime date, TransactionType type, int categoryId, string? note)
        => throw new NotImplementedException();

    public Task DeleteAsync(int id)
        => throw new NotImplementedException();
}
