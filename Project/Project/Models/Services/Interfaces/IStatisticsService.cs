using Project.Models.Entities;

namespace Project.Models.Services.Interfaces;

public interface IStatisticsService
{
    Task<List<MonthlyTotalDto>> GetMonthlyTotalsAsync(int userId, DateTime from);
    Task<List<CategoryBreakdownDto>> GetCategoryBreakdownAsync(int userId, DateTime from);
    Task<List<BalancePointDto>> GetBalanceHistoryAsync(int userId, DateTime from);
}
