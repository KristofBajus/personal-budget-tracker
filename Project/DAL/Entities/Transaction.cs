using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace DAL.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public TransactionType Type { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
