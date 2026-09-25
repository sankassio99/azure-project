## Why

The `webapp/` Angular front-end currently talks to an in-memory HTTP interceptor that mocks expense data and resets on every reload. The `api/PersonalAssistant` .NET project exists but only exposes the default weather-forecast sample endpoint. We need a real, persisted CRUD API for expenses so the front-end can be pointed at an actual backend instead of the mock.

## What Changes

- Add an `Expense` domain entity (`Date`, `Value`, `Description`, `Category`) to the `PersonalAssistant` API.
- Add an EF Core `DbContext` with a SQLite (or in-memory, per design) provider to persist expenses.
- Implement a vertical-slice CRUD feature under `Features/Expenses/` following the repo's Vertical Slice Architecture guidelines:
  - `ListExpenses` (GET all expenses)
  - `CreateExpense` (POST new expense)
  - `UpdateExpense` (PUT update existing expense)
  - `DeleteExpense` (DELETE existing expense)
- Add request validation (FluentValidation) for create/update payloads.
- Register MediatR, FluentValidation, and the `DbContext` in `Program.cs`, replacing/keeping the sample weather-forecast endpoint out of scope.
- Remove the sample `/weatherforecast` endpoint since it is no longer needed as a template reference.

## Capabilities

### New Capabilities
- `expense-tracking-api`: CRUD management of expenses (list, create, update, delete) exposed via a REST API backed by EF Core persistence.

### Modified Capabilities
(none — this is a net-new capability in the API project; no existing specs are modified)

## Impact

- Affected code: `api/PersonalAssistant/PersonalAssistant/` (new `Domain/`, `Features/Expenses/`, `Infrastructure/` folders; changes to `Program.cs` and `PersonalAssistant.csproj`).
- Dependencies: `Microsoft.EntityFrameworkCore.Sqlite` (or `InMemory` for dev), `MediatR`, `FluentValidation.AspNetCore` (or `FluentValidation.DependencyInjectionExtensions`).
- No impact to `webapp/` in this change; wiring the Angular app to the real API is a follow-up change.
