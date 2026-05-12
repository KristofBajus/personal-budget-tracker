# Implementation Roadmap — Personal Budget Tracker

## Phase 1 — Solution Setup ✅
- [x] Create solution file `Project.sln`
- [x] Create MAUI project `Project`
- [x] Create class library project `DAL`
- [x] Add NuGet to DAL: `Microsoft.Data.Sqlite`, `Microsoft.EntityFrameworkCore.Sqlite`, `Microsoft.EntityFrameworkCore.Tools`
- [x] Add NuGet to MAUI: `Microsoft.Data.Sqlite`, `Microsoft.EntityFrameworkCore.Sqlite`, `BCrypt.Net-Next`, `CommunityToolkit.Mvvm`, `CommunityToolkit.Maui`, `LiveChartsCore.SkiaSharpView.Maui`
- [x] Add project reference: MAUI → DAL

## Phase 2 — DAL: Entities & Database ✅
- [x] Create `BaseEntity.cs` (Id, CreatedAt, IsDeleted, DeletedAt)
- [x] Create `User.cs` entity (inherits BaseEntity)
- [x] Create `Category.cs` entity (inherits BaseEntity)
- [x] Create `Transaction.cs` entity (inherits BaseEntity)
- [x] Create `Role.cs` enum (User, Admin)
- [x] Create `TransactionType.cs` enum (Income, Expense)
- [x] Create `AppDbContext.cs` with DbSets, global query filters, relationships
- [x] Add seed data for default categories in DbContext
- [x] Create and run initial migration
- [x] Verify database is created correctly

## Phase 3 — MAUI Project Structure ✅
- [x] Create folder structure: Models/Entities/, Models/Services/, ViewModels/, Views/, Views/Popups/
- [x] Create `UserDto.cs`
- [x] Create `CategoryDto.cs`
- [x] Create `TransactionDto.cs`

## Phase 4 — Color Themes (foundation for all future UI) ✅
- [x] Define light and dark color palettes in `Resources/Styles/Colors.xaml` using `AppThemeBinding`
- [x] Define base control styles in `Resources/Styles/Styles.xaml` (Button, Label, Entry, Frame, etc.)
- [x] Register `CommunityToolkit.Maui` in `MauiProgram.cs`
- [x] Test theme switching manually with `Application.Current.UserAppTheme`

## Phase 5 — Base Infrastructure ✅
- [x] Create `BaseViewModel.cs` (INotifyPropertyChanged via CommunityToolkit.Mvvm)
- [x] Create `SessionService.cs` singleton
- [x] Create service interfaces: `IAuthService`, `ITransactionService`, `ICategoryService`, `IStatisticsService`, `IUserService`, `IProfileService`
- [x] Register DbContext, SessionService, all Services, all ViewModels in `MauiProgram.cs` (DI)

## Phase 6 — Auth: Login & Register ✅
- [x] Implement `AuthService.cs` (Register with BCrypt hash, Login, Logout)
- [x] Create `LoginViewModel.cs`
- [x] Create `LoginPage.xaml` + `LoginPage.xaml.cs`
- [x] Create `RegisterViewModel.cs`
- [x] Create `RegisterPage.xaml` + `RegisterPage.xaml.cs` (includes currency picker: EUR, USD, CZK, GBP, CHF)
- [x] Wire up auto-login after registration
- [x] Handle banned/deleted account messages

## Phase 7 — Shell, Navigation & Dashboard ✅
- [x] Set up `AppShell.xaml` with left flyout sidebar
- [x] Add all routes (Dashboard, Transactions, Categories, Statistics, Profile, Admin)
- [x] Show Admin menu item only when Role == Admin
- [x] Add Logout button at bottom of flyout
- [x] Handle navigation to Login when not authenticated
- [x] Implement `TransactionService.cs` — GetBalance, GetRecent, GetMonthlyTotal
- [x] Create `DashboardViewModel.cs`
- [x] Create `DashboardPage.xaml` — balance card, income/expense cards, this-vs-last card, recent list
- [x] Async data loading on page appear

## Phase 8 — Transactions ✅
- [x] Implement `TransactionService.cs` — GetFiltered, Add, Update, SoftDelete
- [x] Create `TransactionListViewModel.cs` with filter properties
- [x] Create `TransactionListPage.xaml` — filter bar + transaction list
- [x] Create `AddEditTransactionViewModel.cs`
- [x] Create `AddEditTransactionPopup.xaml` (CommunityToolkit.Maui popup)
- [x] Wire up Add button → open popup
- [x] Wire up Edit per row → open popup pre-filled
- [x] Wire up Delete per row → confirmation → soft delete
- [x] Refresh list and dashboard after changes
- [x] Add in-memory search — Entry bound to `SearchText` in ViewModel; matches if note OR category name contains the search string (case-insensitive)

## Phase 9 — Categories ✅
- [x] `CategoryService.cs` — implemented
- [x] `CategoryViewModel.cs` — Add, Edit, Delete commands
- [x] `CategoryPage.xaml` — Income and Expense panels side by side
- [x] Disable Edit/Delete for global (IsGlobal) categories
- [x] Edit shows confirmation warning
- [x] Delete shows blocking message if transactions exist

## Phase 10 — Statistics ✅
- [x] Implement `StatisticsService.cs` — MonthlyTotals, CategoryBreakdown, BalanceHistory
- [x] Create `StatisticsViewModel.cs` with time range selector
- [x] Create `StatisticsPage.xaml` — dropdown + 3 charts + summary card
- [x] Wire LiveCharts2 bar, pie, and line charts
- [x] Async chart data loading, empty state messages

## Phase 11 — Admin ✅
- [x] Add `IsDeleted` field to `UserDto`
- [x] Implement `UserService.cs` — GetAll (IgnoreQueryFilters), Ban, Unban, SoftDelete, GetSystemStats
- [x] Create `AdminDashboardViewModel.cs` — system stats (total/active/banned/deleted users, total transactions, new users this month)
- [x] Create `AdminDashboardPage.xaml` — stat cards overview
- [x] Create `UserManagementViewModel.cs` — search, sort, status filter with `ApplyFilters()`
- [x] Create `UserManagementPage.xaml` — search bar, sort picker, status badges, ban/unban/delete per row
- [x] Prevent admin from deleting themselves
- [x] Soft-deleted rows shown dimmed with Deleted badge

## Phase 12 — Profile ✅
- [x] Implement `ProfileService.cs` — GetProfile, UpdateUsername, UpdateEmail, ChangePassword
- [x] Create `ProfileViewModel.cs`
- [x] Create `ProfilePage.xaml` — display username, email, currency (read-only), joined date; editable fields; change password section
- [x] Change password requires current password confirmation
- [x] Profile link in Shell flyout

## Phase 13 — Import / Export *(optional — skipped, using database instead)*
- [ ] Add export: write user transactions to CSV file (file picker for save location)
- [ ] Add import: read CSV, validate rows, insert transactions
- [ ] Hook into Transaction List page (Export button, Import button)

## Phase 14 — Theme Switcher UI ✅
- [x] App always starts in Auto mode (`UserAppTheme = AppTheme.Unspecified`) — follows OS, no saved state
- [x] Subscribe to `Application.RequestedThemeChanged` — icon in sidebar updates automatically when OS switches while in Auto mode
- [x] Toggle button in Shell flyout footer — shows sun icon (light active) or moon icon (dark active) with "Light"/"Dark" label
- [x] Tapping the toggle switches between Light and Dark (`UserAppTheme`), overriding OS for the session
- [x] No persistence — always resets to Auto on next app launch

## Phase 15 — Demo Seed Data ✅
- [x] `SeedDemoDataAsync()` — runs once on first launch
- [x] Seed admin, demo, student, empty users
- [x] Seed custom categories and 3 months of transactions for demo and student
- [x] Called in `MauiProgram.cs` after `Database.Migrate()`
- [x] `README.md` — prerequisites, how to run, demo credentials, feature list, presentation scenarios

## Phase 16 — Polish & README ✅
- [x] Visual consistency with light and dark themes
- [x] Loading indicators (ActivityIndicator bound to IsBusy) on all pages
- [x] Empty state messages on all list pages
- [x] README complete
