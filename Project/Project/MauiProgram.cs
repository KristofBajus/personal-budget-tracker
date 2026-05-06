using System;
using System.IO;
using CommunityToolkit.Maui;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
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

        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "budget.db");
        builder.Services.AddDbContext<AppDbContext>(
            options => options.UseSqlite($"Data Source={dbPath}"),
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Singleton);

        // Session — Singleton: holds the logged-in user for the entire app lifetime
        builder.Services.AddSingleton<SessionService>();

        // Services
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<ICategoryService, CategoryService>();
        builder.Services.AddTransient<ITransactionService, TransactionService>();
        builder.Services.AddTransient<IStatisticsService, StatisticsService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IProfileService, ProfileService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<CategoryViewModel>();

        // Pages — auth flow
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();

        // Pages — main app
        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<TransactionListPage>();
        builder.Services.AddTransient<CategoryView>();
        builder.Services.AddTransient<StatisticsPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<AdminDashboardPage>();
        builder.Services.AddTransient<UserManagementPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();

        return app;
    }
}
