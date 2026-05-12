namespace Project.Models.Entities;

public class CategoryBreakdownDto
{
    public int CategoryId { get; init; }
    public string Name { get; init; } = null!;
    public string Color { get; init; } = null!;
    public decimal Total { get; init; }
}
