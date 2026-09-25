## Context

`api/PersonalAssistant/PersonalAssistant` is an ASP.NET Core 10 minimal API project scaffolded from the default template (only a `/weatherforecast` sample endpoint, `AddOpenApi`/Swagger already registered). The repo's `.github/instructions/api.instructions.md` mandates Vertical Slice Architecture (VSA): feature folders with MediatR commands/queries, FluentValidation validators, direct `DbContext` access (no generic repositories), and Minimal API endpoints. The `webapp/` Angular front-end already implements the UI against a mocked `/api/expenses` contract (see `webapp/src/app/interceptors/mock-expense.interceptor.ts` and `webapp/src/app/models/expense.model.ts`), so this API should match that shape where practical to ease a future swap.

## Goals / Non-Goals

**Goals:**
- Implement a single `Expenses` vertical slice feature (List, Create, Update, Delete) using MediatR + FluentValidation + Minimal API endpoints, per the repo's VSA guidelines.
- Persist expenses using EF Core with SQLite for local/dev persistence across restarts (simple file-based DB, no external infra required).
- Expose a REST contract compatible with the front-end's expected shape: `id`, `date`, `value`, `description`, `category`.
- Auto-apply EF Core migrations on startup for a smooth local dev experience.

**Non-Goals:**
- No authentication/authorization (single-user personal assistant, out of scope for this change).
- No wiring of the Angular app to this real API (follow-up change).
- No pagination, filtering, or sorting beyond returning the full list ordered by date descending.
- No multi-currency support; `Value` is a single `decimal`.

## Decisions

- **Architecture**: Follow Vertical Slice Architecture per `.github/instructions/api.instructions.md`: `Features/Expenses/ListExpenses/`, `Features/Expenses/CreateExpense/`, `Features/Expenses/UpdateExpense/`, `Features/Expenses/DeleteExpense/`, each with its own MediatR command/query, handler, validator (for writes), and Minimal API endpoint registration. Rationale: matches explicit repo convention; keeps slices independent and easy to test.
- **Persistence**: EF Core with `Microsoft.EntityFrameworkCore.Sqlite`, storing a local `personalassistant.db` file. Rationale: zero external infrastructure, persists across restarts (better than `InMemory` provider for a real demo), still simple to set up. Alternative considered: `InMemory` provider — rejected because data would reset on every restart, undermining the point of a "real backend."
- **Entity**: `Expense` record/class with `Id` (int, identity), `Date` (`DateOnly`), `Value` (`decimal`), `Description` (`string`, required, max length), `Category` (`string`, required, max length). Rationale: matches the four fields required by the proposal; `DateOnly` avoids time-of-day ambiguity; `decimal` is required by repo conventions for monetary values.
- **DbContext**: Single `PersonalAssistantDbContext` in `Infrastructure/Data/`, registered via `AddDbContext` with SQLite connection string from configuration (`appsettings.json`), migrations applied automatically at startup in `Program.cs` for dev simplicity.
- **CQRS via MediatR**: `ListExpensesQuery` (no params) returns `IReadOnlyList<ExpenseResponse>`; `CreateExpenseCommand`, `UpdateExpenseCommand`, `DeleteExpenseCommand` return the affected `ExpenseResponse` (or `Guid`/`int` id, or `NotFound` result for update/delete). Rationale: matches repo's CQRS requirement.
- **Validation**: FluentValidation validators for `CreateExpenseCommand`/`UpdateExpenseCommand` enforcing non-empty `Description`/`Category`, non-default `Date`, and `Value` not equal to zero (allow negative for refunds/adjustments, per no explicit constraint from proposal). Validation runs via a MediatR pipeline behavior (`ValidationBehavior<TRequest,TResponse>`) so handlers stay free of manual validation blocks.
- **Endpoints**: Minimal API group `app.MapGroup("/api/expenses")` with `GET /`, `POST /`, `PUT /{id}`, `DELETE /{id}`, matching the mock interceptor's URL shape used by the front-end. Endpoints return `ProblemDetails`-style responses for validation failures (400) and not-found (404).
- **Remove sample code**: Delete the `/weatherforecast` endpoint and `WeatherForecast` record from `Program.cs` since it's unrelated template scaffolding.

## Risks / Trade-offs

- [SQLite file lives alongside the app; not suitable for multi-instance/cloud deployment] → Acceptable for a personal-assistant single-instance app; document as a future migration to a hosted DB (e.g., Azure SQL) if needed.
- [Auto-applying migrations on startup can be risky in production] → Acceptable for this personal project's dev/single-user scope; guarded behind a straightforward `Database.Migrate()` call that is easy to replace with a proper deploy-time migration step later.
- [Front-end still uses mock data after this change, so the two are not yet wired together] → Explicitly a non-goal here; tracked as a follow-up change to avoid scope creep.

## Open Questions

- None blocking; proceed with the above decisions.
