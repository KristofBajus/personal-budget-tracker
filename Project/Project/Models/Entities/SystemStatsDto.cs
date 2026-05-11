namespace Project.Models.Entities;

public class SystemStatsDto
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int BannedUsers { get; set; }
    public int DeletedUsers { get; set; }
    public int TotalTransactions { get; set; }
    public int NewUsersThisMonth { get; set; }
}
