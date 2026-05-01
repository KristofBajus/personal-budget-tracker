using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL;

// Used only by EF Core CLI tools (dotnet ef migrations) — not used at runtime
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=budget_designtime.db")
            .Options;

        return new AppDbContext(options);
    }
}
