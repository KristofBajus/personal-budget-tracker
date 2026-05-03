namespace Project.Models.Entities;

public class CategoryBreakdownDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public decimal Total { get; set; }
}
