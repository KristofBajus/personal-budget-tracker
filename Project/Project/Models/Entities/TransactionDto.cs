using System;
using DAL.Enums;

namespace Project.Models.Entities;

public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public string? Note { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string CategoryColor { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public bool IsIncome => Type == TransactionType.Income;
    public string FormattedAmount => IsIncome ? $"+{Amount:N2}" : $"-{Amount:N2}";
}
