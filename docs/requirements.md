# Requirements — Personal Budget Tracker

## Functional Requirements

### Authentication
- User can register with username, email, and password
- Password is never stored in plain text — hashed with BCrypt
- After successful registration, user is automatically logged in
- User can log in and log out
- Banned account shows explicit "suspended" message on login attempt
- Soft-deleted account shows generic "invalid credentials" message

### Transactions
- User can add a transaction (income or expense)
- Each transaction contains: amount, date, type (Income/Expense), category, optional note
- User can edit any of their transactions
- User can delete a transaction (soft delete — data is preserved in DB)
- Transactions are displayed in a filterable list

### Filtering
- Transactions can be filtered by: type, category, month, year
- Filters can be combined

### Categories
- App ships with pre-seeded default categories (read-only for users):
  - Income: Salary, Freelance, Gift, Allowance, Other
  - Expense: Food, Housing, Transport, Health, Entertainment, Shopping, Other
- User can create custom categories for both income and expenses
- User can only edit and delete their own custom categories — global (pre-seeded) categories are read-only for all users
- User can edit a custom category name — a confirmation warning is shown since it affects existing transactions
- User can delete a custom category only if no transactions reference it — otherwise a blocking message is shown
- Category deletion is a soft delete

### Dashboard
- Shows current account balance (sum of all incomes minus all expenses)
- Shows a list of recent transactions

### Statistics
- Time range selector: Last 30 days / Last 3 months / Last 6 months / This year / All time
- Monthly income vs expense bar chart
- Spending breakdown by category (pie/donut chart)
- Running balance over time (line chart)
- This month vs last month summary card (numbers only)

### Import / Export
- User can export their transactions to a CSV file
- User can import transactions from a CSV file

### Profile
- User can view their own profile (username, email, joined date)
- User can edit their username and email
- User can change their password (requires current password confirmation)
- Only the logged-in user can edit their own profile

### Admin Panel
- Admin can view a list of all users (including banned and soft-deleted) with status badges
- Admin can ban a user
- Admin can unban a user
- Admin can soft-delete a user account
- Soft-deleted usernames remain reserved — no new registration with the same username is allowed
- Admin cannot view personal transaction data of other users

---

## Non-Functional Requirements

- All database operations are asynchronous (async/await throughout)
- Passwords are hashed using BCrypt — never stored or logged in plain text
- Soft delete is used on all entities — records are never permanently removed from DB
- All entities store CreatedAt timestamp; soft-deleted entities also store DeletedAt
- Strict MVVM — no business logic in Views or code-behind files
- Session state is managed via a singleton SessionService injected through DI
- Application is designed for desktop (Windows) but layout is responsive
- README contains all information needed to build and run the application
