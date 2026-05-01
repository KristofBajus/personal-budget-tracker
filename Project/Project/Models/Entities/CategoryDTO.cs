using DAL.Enums;

namespace Project.Models.Entities;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public TransactionType Type { get; set; }
    public bool IsGlobal { get; set; }
    public int? UserId { get; set; }
}
