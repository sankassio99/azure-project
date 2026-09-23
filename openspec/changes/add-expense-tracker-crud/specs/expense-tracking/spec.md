## ADDED Requirements

### Requirement: List Expenses
The system SHALL display a list of all expenses, each showing date, value, description, and category.

#### Scenario: Expenses are loaded and displayed
- **WHEN** the user navigates to the expense list view
- **THEN** the system fetches expenses via HTTP and displays them in a list, each row showing date, value, description, and category

#### Scenario: No expenses exist
- **WHEN** the expense list is empty
- **THEN** the system displays an empty-state message instead of a list

### Requirement: Add Expense
The system SHALL allow the user to create a new expense with date, value, description, and category.

#### Scenario: Successful creation
- **WHEN** the user submits the add-expense form with a valid date, value, description, and category
- **THEN** the system sends an HTTP create request, adds the new expense to the list, and navigates back to the list view

#### Scenario: Invalid submission
- **WHEN** the user submits the add-expense form with a missing or invalid required field
- **THEN** the system prevents submission and displays a validation error for each invalid field

### Requirement: Edit Expense
The system SHALL allow the user to update an existing expense's date, value, description, and category.

#### Scenario: Successful edit
- **WHEN** the user modifies an existing expense's fields and submits the edit form with valid data
- **THEN** the system sends an HTTP update request, updates the expense in the list, and navigates back to the list view

#### Scenario: Editing a non-existent expense
- **WHEN** the user navigates to edit an expense id that no longer exists
- **THEN** the system displays a not-found message and offers navigation back to the list

### Requirement: Delete Expense
The system SHALL allow the user to delete an existing expense after confirmation.

#### Scenario: Confirmed deletion
- **WHEN** the user requests to delete an expense and confirms the action
- **THEN** the system sends an HTTP delete request and removes the expense from the displayed list

#### Scenario: Cancelled deletion
- **WHEN** the user requests to delete an expense but cancels the confirmation
- **THEN** the system makes no HTTP request and the expense remains in the list

### Requirement: Mock HTTP Data Layer
The system SHALL serve expense data through an `HttpClient`-compatible mock backend so all CRUD operations exercise real HTTP request/response flows without a live server.

#### Scenario: Reading initial data
- **WHEN** the application starts and the expense list is requested
- **THEN** the mock HTTP layer returns a seeded set of sample expenses
