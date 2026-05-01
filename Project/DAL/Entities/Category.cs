using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace DAL.Entities;

public class Category : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    // Hex color used for category badges in the transaction list and chart slices in Statistics
    [MaxLength(7)]
    public string Color { get; set; } = "#9E9E9E";

    public TransactionType Type { get; set; }

    public bool IsGlobal { get; set; }

    // Null = pre-seeded global category (read-only for all users); set = user-created category
    public int? UserId { get; set; }
    public User? User { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = [];
}
