using Project.Models.Entities;
using Project.Models.Services.Interfaces;

namespace Project.Models.Services;

public class StatisticsService : IStatisticsService
{
    public Task<List<MonthlyTotalDto>> GetMonthlyTotalsAsync(int userId, DateTime from)
        => throw new NotImplementedException();

    public Task<List<CategoryBreakdownDto>> GetCategoryBreakdownAsync(int userId, DateTime from)
        => throw new NotImplementedException();

    public Task<List<BalancePointDto>> GetBalanceHistoryAsync(int userId, DateTime from)
        => throw new NotImplementedException();
}
