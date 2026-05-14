# Technology Stack — Personal Budget Tracker

## .NET MAUI
**Used for:** The entire UI layer — pages, layouts, controls, navigation.
**Why:** Cross-platform framework for C#. Allows building native Mac Catalyst and iOS apps while keeping the option to target other platforms. Shell provides built-in navigation with a left-side flyout.

## Entity Framework Core (EF Core) + SQLite
**Used for:** All database access — defining models, running migrations, querying and persisting data.
**Why:** EF Core lets us write LINQ queries against a typed DbContext instead of raw SQL. SQLite is a lightweight file-based database — ideal for a local desktop app with no server needed.
**NuGet packages:**
- `Microsoft.Data.Sqlite`
- `Microsoft.EntityFrameworkCore.Sqlite`
- `Microsoft.EntityFrameworkCore.Tools` (migrations)

## Separate DAL Class Library (`DAL`)
**Used for:** Isolating all database concerns — EF entities, DbContext, migrations, and seed data.
**Why:** Keeps the MAUI project clean. MAUI only works with DTOs and Services, never with EF entities directly. This separation also makes the data layer independently testable.

## CommunityToolkit.Mvvm
**Used for:** ViewModel base classes, `[ObservableProperty]`, `[RelayCommand]` source generators.
**Why:** Eliminates boilerplate INotifyPropertyChanged implementation. Lets ViewModels be written cleanly with minimal ceremony.

## CommunityToolkit.Maui
**Used for:** Popup dialogs (Add/Edit Transaction modal).
**Why:** MAUI does not have a built-in popup/modal control. CommunityToolkit.Maui adds this cleanly without needing third-party workarounds.

## BCrypt.Net-Next
**Used for:** Hashing and verifying user passwords.
**Why:** BCrypt is a well-established, slow hashing algorithm designed for passwords. It automatically handles salting and is resistant to brute-force attacks. Passwords are never stored or compared in plain text.
**NuGet package:** `BCrypt.Net-Next`

## LiveCharts2
**Used for:** Rendering charts on the Statistics page — bar chart, pie/donut chart, line chart.
**Why:** MAUI has no built-in charting. LiveCharts2 has MAUI support and covers all three chart types needed.
**NuGet package:** `LiveChartsCore.SkiaSharpView.Maui`

## Dependency Injection (built-in .NET DI)
**Used for:** Registering and resolving Services, ViewModels, DbContext, and SessionService throughout the app.
**Why:** Standard .NET DI is already integrated into MAUI via `MauiProgram.cs`. Keeps components loosely coupled and makes testing easier.

## SessionService (custom singleton)
**Used for:** Holding the currently logged-in user across all pages and ViewModels.
**Why:** MAUI has no built-in concept of "current user". A singleton service registered in DI is the cleanest way to share auth state without global static variables.

## async / await
**Used for:** All database read and write operations.
**Why:** Required by the assignment and good practice — keeps the UI thread unblocked so the app stays responsive while waiting for DB operations.
