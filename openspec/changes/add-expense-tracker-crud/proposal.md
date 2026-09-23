## Why

There is no application in this workspace yet. We need a simple, focused front-end to demonstrate a full CRUD workflow and full-stack-style architecture (component + service layer) using modern Angular, backed by mock data so it can be built and tested standalone before any real backend exists.

## What Changes

- Scaffold a new Angular 21 application in `webapp/` using standalone components, signals, and the new control flow.
- Add an `Expense` model with `date`, `value`, `description`, and `category` fields.
- Add an `ExpenseService` that exposes HTTP-based CRUD operations (list, add, edit, delete) backed by mock data via `HttpClient` and an `HttpInterceptor` (or in-memory web API) for testing without a real backend.
- Add an expense list view showing all expenses.
- Add an add/edit expense form (shared component) for creating and updating expenses.
- Add delete functionality with confirmation.
- Add basic client-side routing between the list view and the add/edit view.

## Capabilities

### New Capabilities
- `expense-tracking`: CRUD management of expenses (list, create, update, delete) via an HTTP-backed service using mock data, exposed through Angular components.

### Modified Capabilities
(none — this is a net-new application)

## Impact

- Affected code: new `webapp/` Angular application (all new files).
- Dependencies: Angular 21, `@angular/common/http` for the mock HTTP layer.
- No existing systems affected since this is the first feature in the workspace.
