using System;
using DAL.Enums;

namespace Project.Models.Entities;

public class UserDto
{
    public int Id { get; init; }
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Currency { get; init; } = null!;
    public Role Role { get; init; }
    public bool IsBanned { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime CreatedAt { get; init; }

    public bool IsActive => !IsBanned && !IsDeleted;
    public string StatusText => IsDeleted ? "Deleted" : IsBanned ? "Banned" : "Active";
}
