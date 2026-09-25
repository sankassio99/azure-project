## 1. Project Dependencies & Setup

- [x] 1.1 Add NuGet packages to `PersonalAssistant.csproj`: `Microsoft.EntityFrameworkCore.Sqlite`, `Microsoft.EntityFrameworkCore.Design`, `MediatR`, `FluentValidation.DependencyInjectionExtensions`
- [x] 1.2 Add SQLite connection string to `appsettings.json` / `appsettings.Development.json`

## 2. Domain & Persistence

- [x] 2.1 Create `Domain/Entities/Expense.cs` with `Id`, `Date` (`DateOnly`), `Value` (`decimal`), `Description` (`string`), `Category` (`string`)
- [x] 2.2 Create `Infrastructure/Data/PersonalAssistantDbContext.cs` with a `DbSet<Expense>` and entity configuration (required fields, max lengths)
- [x] 2.3 Register `PersonalAssistantDbContext` with the SQLite provider in `Program.cs`
- [ ] 2.4 Add initial EF Core migration for the `Expenses` table
- [x] 2.5 Apply pending migrations automatically at app startup in `Program.cs`

## 3. Cross-Cutting Infrastructure

- [x] 3.1 Register MediatR (scanning the assembly) in `Program.cs`
- [x] 3.2 Register FluentValidation validators (scanning the assembly) in `Program.cs`
- [x] 3.3 Add a MediatR pipeline behavior (`ValidationBehavior<TRequest,TResponse>`) that runs registered validators and throws/returns a validation failure before the handler executes
- [x] 3.4 Add a shared `ExpenseResponse` record used across the feature slices

## 4. Feature Slice: List Expenses

- [x] 4.1 Create `Features/Expenses/ListExpenses/ListExpensesQuery.cs` (MediatR query, no parameters)
- [x] 4.2 Create `Features/Expenses/ListExpenses/ListExpensesHandler.cs` using `AsNoTracking()` and projecting to `ExpenseResponse`, ordered by `Date` descending
- [x] 4.3 Create `Features/Expenses/ListExpenses/ListExpensesEndpoint.cs` mapping `GET /api/expenses`

## 5. Feature Slice: Create Expense

- [x] 5.1 Create `Features/Expenses/CreateExpense/CreateExpenseCommand.cs` (MediatR command with `Date`, `Value`, `Description`, `Category`)
- [x] 5.2 Create `Features/Expenses/CreateExpense/CreateExpenseValidator.cs` (FluentValidation: non-default `Date`, non-zero `Value`, non-empty `Description`/`Category` with max length)
- [x] 5.3 Create `Features/Expenses/CreateExpense/CreateExpenseHandler.cs` persisting the new `Expense` and returning `ExpenseResponse`
- [x] 5.4 Create `Features/Expenses/CreateExpense/CreateExpenseEndpoint.cs` mapping `POST /api/expenses`, returning `201 Created`

## 6. Feature Slice: Update Expense

- [x] 6.1 Create `Features/Expenses/UpdateExpense/UpdateExpenseCommand.cs` (MediatR command with `Id`, `Date`, `Value`, `Description`, `Category`)
- [x] 6.2 Create `Features/Expenses/UpdateExpense/UpdateExpenseValidator.cs` (same field rules as create)
- [x] 6.3 Create `Features/Expenses/UpdateExpense/UpdateExpenseHandler.cs` loading the existing `Expense` by id, returning not-found when missing, otherwise updating and returning `ExpenseResponse`
- [x] 6.4 Create `Features/Expenses/UpdateExpense/UpdateExpenseEndpoint.cs` mapping `PUT /api/expenses/{id}`, returning `200 OK` or `404 Not Found`

## 7. Feature Slice: Delete Expense

- [x] 7.1 Create `Features/Expenses/DeleteExpense/DeleteExpenseCommand.cs` (MediatR command with `Id`)
- [x] 7.2 Create `Features/Expenses/DeleteExpense/DeleteExpenseHandler.cs` removing the `Expense` by id, returning not-found when missing
- [x] 7.3 Create `Features/Expenses/DeleteExpense/DeleteExpenseEndpoint.cs` mapping `DELETE /api/expenses/{id}`, returning `204 No Content` or `404 Not Found`

## 8. Cleanup

- [x] 8.1 Remove the sample `/weatherforecast` endpoint and `WeatherForecast` record from `Program.cs`
- [ ] 8.2 Update `PersonalAssistant.http` with sample requests for the new expense endpoints

## 9. Verification

- [ ] 9.1 Run `dotnet build` to confirm the project compiles without errors
- [ ] 9.2 Manually verify list, create, update, delete flows via `PersonalAssistant.http` or Swagger UI
- [ ] 9.3 Verify validation errors return `400` and unknown ids return `404` for update/delete
