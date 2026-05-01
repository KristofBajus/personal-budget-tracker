using DAL.Enums;

namespace Project.Models.Entities;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public Role Role { get; set; }
    public bool IsBanned { get; set; }
    public DateTime CreatedAt { get; set; }
}
