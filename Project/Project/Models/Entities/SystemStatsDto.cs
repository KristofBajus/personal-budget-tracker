namespace Project.Models.Entities;

public class SystemStatsDto
{
    public int TotalUsers { get; init; }
    public int ActiveUsers { get; init; }
    public int BannedUsers { get; init; }
    public int DeletedUsers { get; init; }
    public int TotalTransactions { get; init; }
    public int NewUsersThisMonth { get; init; }
}
