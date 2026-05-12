using DAL.Enums;

namespace Project.Models.Entities;

public class CategoryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Color { get; init; } = null!;
    public TransactionType Type { get; init; }
    public bool IsGlobal { get; init; }
    public int? UserId { get; init; }
}
