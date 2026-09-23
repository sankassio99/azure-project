## 1. Scaffold Angular Application

- [x] 1.1 Generate a new Angular 21 standalone application in `webapp/` using the Angular CLI (routing enabled, CSS stylesheets)
- [ ] 1.2 Verify the app builds and serves with the default starter page (skipped for this session per user request — no terminal builds)

## 2. Expense Model & Mock Data

- [x] 2.1 Create `Expense` interface (`id`, `date`, `value`, `description`, `category`)
- [x] 2.2 Create a seeded array of sample mock expenses for initial data

## 3. Mock HTTP Backend

- [x] 3.1 Implement an `HttpInterceptorFn` that intercepts requests to `/api/expenses` (and `/api/expenses/:id`) for GET, POST, PUT, DELETE
- [x] 3.2 Back the interceptor with an in-memory store seeded from the mock data, simulating latency and returning appropriate HTTP status codes
- [x] 3.3 Register the interceptor via `provideHttpClient(withInterceptors([...]))` in `app.config.ts`

## 4. Expense Service

- [x] 4.1 Create `ExpenseService` using `inject(HttpClient)` with a `signal<Expense[]>` cache
- [x] 4.2 Implement `list()` to GET `/api/expenses` and populate the signal
- [x] 4.3 Implement `add(expense)` to POST a new expense and update the signal
- [x] 4.4 Implement `update(expense)` to PUT an updated expense and update the signal
- [x] 4.5 Implement `remove(id)` to DELETE an expense and update the signal
- [x] 4.6 Implement `getById(id)` helper (via `computed()` over the cached signal or a dedicated GET)

## 5. Routing

- [ ] 5.1 Configure routes: `/expenses` (list, default redirect), `/expenses/new` (add form), `/expenses/:id/edit` (edit form), using `loadComponent`

## 6. Expense List Component

- [ ] 6.1 Create standalone `ExpenseListComponent` with inline template using `@for`/`@if` to render expenses
- [ ] 6.2 Add "Add Expense" navigation button and per-row Edit/Delete actions
- [ ] 6.3 Implement delete confirmation (e.g., native `confirm()` or a simple confirm UI) before calling `remove()`
- [ ] 6.4 Add empty-state message when there are no expenses

## 7. Expense Form Component

- [ ] 7.1 Create standalone `ExpenseFormComponent` (shared for add/edit) using Reactive Forms with fields for date, value, description, category
- [ ] 7.2 Add required-field validation and display validation error messages
- [ ] 7.3 On init, read an optional route `id` param; if present, load the expense and patch the form for edit mode
- [ ] 7.4 On submit, call `add()` or `update()` depending on mode, then navigate back to the list

## 8. Styling & Accessibility

- [ ] 8.1 Add component-scoped CSS for list and form layout (flex/grid, simple spacing)
- [ ] 8.2 Ensure form fields have associated `<label>`s and inputs use proper `type`/`aria-*` attributes
- [ ] 8.3 Verify focus management on navigation between list and form views

## 9. Verification

- [ ] 9.1 Manually verify list, add, edit, delete flows work end-to-end against the mock HTTP layer
- [ ] 9.2 Run `ng build` to confirm the app compiles without errors
