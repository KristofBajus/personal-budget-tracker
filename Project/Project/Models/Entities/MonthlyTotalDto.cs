namespace Project.Models.Entities;

public class MonthlyTotalDto
{
    public int Year { get; init; }
    public int Month { get; init; }
    public decimal Income { get; init; }
    public decimal Expense { get; init; }
}
