using CommunityToolkit.Maui;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project.Models.Services;
using Project.Models.Services.Interfaces;
using Project.ViewModels;
using Project.Views;

namespace Project;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Database — Transient so each service gets its own DbContext instance
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "budget.db");
        builder.Services.AddDbContext<AppDbContext>(
            options => options.UseSqlite($"Data Source={dbPath}"),
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Singleton);

        // Session — Singleton: one instance holds the logged-in user for the entire app lifetime
        builder.Services.AddSingleton<SessionService>();

        // Services
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<ICategoryService, CategoryService>();
        builder.Services.AddTransient<ITransactionService, TransactionService>();
        builder.Services.AddTransient<IStatisticsService, StatisticsService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IProfileService, ProfileService>();

        // ViewModels
        builder.Services.AddTransient<CategoryViewModel>();

        // Pages
        builder.Services.AddTransient<CategoryView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Run DB migration once on startup
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();

        return app;
    }
}
