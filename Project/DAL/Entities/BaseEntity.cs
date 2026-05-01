namespace DAL.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // Soft delete: records are never removed from the DB; IsDeleted=true hides them via global query filters in AppDbContext
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}
