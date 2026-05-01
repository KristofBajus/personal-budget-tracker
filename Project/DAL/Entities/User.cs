using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace DAL.Entities;

public class User : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    public Role Role { get; set; } = Role.User;

    public bool IsBanned { get; set; } = false;

    // Set once at registration, never editable afterward
    [Required]
    [MaxLength(10)]
    public string Currency { get; set; } = "EUR";

    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<Category> Categories { get; set; } = [];
}
