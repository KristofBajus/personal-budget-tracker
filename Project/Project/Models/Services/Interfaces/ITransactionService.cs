using DAL.Enums;
using Project.Models.Entities;

namespace Project.Models.Services.Interfaces;

public interface ITransactionService
{
    Task<List<TransactionDto>> GetFilteredAsync(int userId, TransactionType? type = null, int? categoryId = null, int? month = null, int? year = null);
    Task<List<TransactionDto>> GetRecentAsync(int userId, int count = 10);
    Task<decimal> GetBalanceAsync(int userId);
    Task<decimal> GetMonthlyTotalAsync(int userId, TransactionType type, int year, int month);
    Task<TransactionDto> AddAsync(int userId, decimal amount, DateTime date, TransactionType type, int categoryId, string? note);
    Task UpdateAsync(int id, decimal amount, DateTime date, TransactionType type, int categoryId, string? note);
    Task DeleteAsync(int id);
}
