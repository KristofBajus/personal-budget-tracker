using System;
using DAL.Enums;

namespace Project.Models.Entities;

public class TransactionDto
{
    public int Id { get; init; }
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public TransactionType Type { get; init; }
    public string? Note { get; init; }
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
    public string CategoryColor { get; init; } = null!;
    public DateTime CreatedAt { get; init; }

    public bool IsIncome => Type == TransactionType.Income;
    public string FormattedAmount => IsIncome ? $"+{Amount:N2}" : $"-{Amount:N2}";
}
