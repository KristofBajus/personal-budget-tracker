# Screens — Personal Budget Tracker

## Navigation Structure

Left-side Shell flyout (visible after login):
- Dashboard
- Transactions
- Categories
- Statistics
- Admin *(visible only to admin role)*
- Profile
- Logout button at bottom

Login and Register pages are shown before the Shell — user cannot access the flyout without being authenticated.

---

## 1. Login Page

**Route:** shown on app start if no user is logged in

**Layout:** Centered card on a clean background.

**Elements:**
- App name / logo at top
- Username field
- Password field
- Login button
- Link to Register page

**Behavior:**
- On success → navigates to Dashboard, Shell flyout becomes visible
- If account is banned → shows "Your account has been suspended" error message
- If account deleted or wrong credentials → shows generic "Invalid username or password"
- Fields validated before submission (no empty fields)

---

## 2. Register Page

**Route:** accessible from Login page

**Layout:** Same centered card style as Login.

**Elements:**
- Username field
- Email field
- Password field
- Confirm password field
- Register button
- Link back to Login

**Behavior:**
- Validates all fields (no empty, passwords match, email format)
- Username uniqueness checked — reserved even for soft-deleted accounts
- On success → auto-login, navigate to Dashboard

---

## 3. Dashboard Page

**Route:** default page after login

**Layout:** Two-column desktop layout. Left: summary cards. Right: recent transactions list.

**Elements:**
- Current balance card (large, prominent) — total incomes minus total expenses
- This month income card
- This month expense card
- This month vs last month comparison card (e.g. "+12% spending vs last month")
- Recent transactions list (last ~10 entries) with amount, category, date

**Behavior:**
- All data scoped to the logged-in user
- Balance and cards update when transactions change
- Clicking a transaction in the recent list opens the Edit popup

---

## 4. Transaction List Page

**Route:** Transactions in sidebar

**Layout:** Filter bar at top, scrollable transaction list below.

**Elements:**
- Filter bar: Type dropdown (All/Income/Expense), Category dropdown, Month picker, Year picker, Clear filters button
- Transaction list rows: date, category icon/color, note preview, amount (green for income, red for expense)
- Add Transaction button (floating or top-right)
- Edit and Delete buttons per row (or on row click)

**Behavior:**
- Filters are applied in combination, results update immediately
- Add button opens Add/Edit Transaction popup
- Edit button on a row opens the same popup pre-filled
- Delete shows a confirmation dialog before soft-deleting
- Soft-deleted transactions disappear from the list

---

## 5. Add / Edit Transaction Popup

**Type:** Modal popup (CommunityToolkit.Maui)

**Elements:**
- Title: "Add Transaction" or "Edit Transaction"
- Amount field (numeric)
- Date picker (defaults to today)
- Type selector (Income / Expense toggle or dropdown)
- Category dropdown (filtered to match selected type)
- Note field (optional, multiline)
- Save button
- Cancel button

**Behavior:**
- Changing type refreshes the category list to show only matching categories
- Validates amount > 0, date set, category selected
- On save → closes popup, transaction list and dashboard refresh

---

## 6. Category Page

**Route:** Categories in sidebar

**Layout:** Two panels side by side — Income categories left, Expense categories right. Or tabs.

**Elements:**
- Income categories list
- Expense categories list
- Each category row shows name, type badge, and Edit/Delete buttons
- Default/seeded categories shown but Edit and Delete disabled for them
- Add Category button per section (or one button with type selector)

**Behavior:**
- Edit opens an inline edit field or small popup — on save shows warning "This will rename the category in all existing transactions. Continue?"
- Delete blocked if transactions reference the category — shows message "Cannot delete: X transactions use this category"
- Delete on unused category shows confirmation, then soft-deletes

---

## 7. Statistics Page

**Route:** Statistics in sidebar

**Layout:** Time range selector at top. Charts stacked or in a 2x2 grid below.

**Elements:**
- Time range dropdown: Last 30 days / Last 3 months / Last 6 months / This year / All time
- Monthly income vs expense — bar chart (one bar pair per month)
- Spending by category — pie/donut chart (expenses only, colored by category)
- Balance over time — line chart (running total by day or month)
- This month vs last month — summary card with percentage difference

**Behavior:**
- All charts react to the time range selection
- Charts are rendered asynchronously
- If no data exists for the range, show a friendly empty state message

---

## 8. Admin Page

**Route:** Admin in sidebar (visible only if Role == Admin)

**Layout:** Full-width user table.

**Elements:**
- User list table: Username, Email, Role, Created date, Status badge (Active / Banned / Deleted)
- Per-row actions: Ban / Unban button, Delete button
- Deleted users shown with dimmed row style
- Filter/search by username (optional, nice to have)

**Behavior:**
- Ban sets IsBanned = true, badge changes to Banned, button switches to Unban
- Unban reverses it
- Delete shows confirmation dialog, then soft-deletes — row stays visible with Deleted badge
- Admin cannot delete themselves
- Admin cannot see transaction data of other users

---

## 9. Profile Page

**Route:** Profile in sidebar

**Layout:** Single centered card with user info and editable sections.

**Elements:**
- Username display (editable)
- Email display (editable)
- Joined date (read-only)
- Save profile button
- Separator
- Change password section: Current password, New password, Confirm new password fields
- Change password button

**Behavior:**
- Save profile validates username uniqueness (excluding soft-deleted accounts)
- Change password requires correct current password before accepting the new one
- Success/error messages shown inline after each action
- Only the logged-in user can edit their own profile — no admin access to other profiles
