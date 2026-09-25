## ADDED Requirements

### Requirement: List Expenses
The system SHALL provide an endpoint that returns all expenses, each including `id`, `date`, `value`, `description`, and `category`, ordered by `date` descending.

#### Scenario: Expenses exist
- **WHEN** a client sends `GET /api/expenses` and one or more expenses have been created
- **THEN** the system responds with `200 OK` and a JSON array of all expenses ordered by `date` descending

#### Scenario: No expenses exist
- **WHEN** a client sends `GET /api/expenses` and no expenses have been created
- **THEN** the system responds with `200 OK` and an empty JSON array

### Requirement: Create Expense
The system SHALL allow a client to create a new expense by providing `date`, `value`, `description`, and `category`, and SHALL reject invalid input.

#### Scenario: Valid expense submitted
- **WHEN** a client sends `POST /api/expenses` with a valid `date`, non-zero `value`, non-empty `description`, and non-empty `category`
- **THEN** the system persists the expense, assigns it a unique `id`, and responds with `201 Created` including the created expense

#### Scenario: Invalid expense submitted
- **WHEN** a client sends `POST /api/expenses` with a missing/empty `description` or `category`, a zero `value`, or a missing `date`
- **THEN** the system responds with `400 Bad Request` and does not persist any expense

### Requirement: Update Expense
The system SHALL allow a client to update an existing expense's `date`, `value`, `description`, and `category` by `id`, and SHALL reject invalid input or unknown ids.

#### Scenario: Valid update to an existing expense
- **WHEN** a client sends `PUT /api/expenses/{id}` with valid field values and `{id}` matches an existing expense
- **THEN** the system updates the stored expense and responds with `200 OK` including the updated expense

#### Scenario: Update targets a non-existent expense
- **WHEN** a client sends `PUT /api/expenses/{id}` and no expense with `{id}` exists
- **THEN** the system responds with `404 Not Found` and makes no changes

#### Scenario: Invalid update payload
- **WHEN** a client sends `PUT /api/expenses/{id}` with a missing/empty `description` or `category`, a zero `value`, or a missing `date`
- **THEN** the system responds with `400 Bad Request` and makes no changes

### Requirement: Delete Expense
The system SHALL allow a client to delete an existing expense by `id`, and SHALL respond appropriately when the `id` does not exist.

#### Scenario: Delete an existing expense
- **WHEN** a client sends `DELETE /api/expenses/{id}` and `{id}` matches an existing expense
- **THEN** the system removes the expense and responds with `204 No Content`

#### Scenario: Delete targets a non-existent expense
- **WHEN** a client sends `DELETE /api/expenses/{id}` and no expense with `{id}` exists
- **THEN** the system responds with `404 Not Found` and makes no changes
