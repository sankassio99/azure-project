---
description: "Vertical Slice Architecture guidelines for .NET APIs"
applyTo: '**/*.cs,**/*.csproj,**/Program.cs,**/*.razor'
---

# Vertical Slice Architecture Guidelines

You are an expert .NET backend developer specializing in Vertical Slice Architecture (VSA). When writing, modifying, or refactoring code, follow these rules.

## Core Principles

1. **Organize by business capability**: Structure code by feature rather than technical layer. Use paths such as `Features/[FeatureGroup]/[FeatureName]/`.
2. **Keep slices self-contained**: Place the endpoint, request, response, MediatR command or query, validator, and handler together in the same feature folder or file.
3. **Minimize abstractions**: Do not create generic repositories or Unit of Work wrappers. Query or mutate the `DbContext` directly inside the feature handler.
4. **Keep slices independent**: Avoid dependencies between feature slices. Put genuinely shared domain rules in domain entities or shared infrastructure only when necessary.
5. **Use CQRS with MediatR**: Separate read operations into queries and write operations into commands.

## SOLID Principles

- **Single Responsibility**: Keep each handler, validator, and endpoint focused on one reason to change.
- **Open/Closed**: Extend behavior through new handlers, validators, or pipeline behaviors rather than modifying existing ones.
- **Liskov Substitution**: Ensure any interface implementation (e.g., pipeline behaviors, domain abstractions) can substitute for its abstraction without breaking callers.
- **Interface Segregation**: Keep interfaces small and specific to the consumer's needs; avoid forcing handlers to depend on members they don't use.
- **Dependency Inversion**: Depend on abstractions (e.g., `IMediator`, `DbContext` as an injected dependency) rather than concrete implementations; use constructor injection.

## .NET Best Practices

- Use `async`/`await` for all I/O-bound operations end-to-end; avoid blocking calls like `.Result` or `.Wait()`.
- Use constructor injection and the built-in DI container; avoid service locator patterns.
- Prefer modern C# features (records, pattern matching, nullable reference types, primary constructors) for concise, robust code.
- Handle exceptions centrally (e.g., middleware or MediatR pipeline behaviors) rather than scattering try/catch blocks across handlers.
- Use `CancellationToken` parameters through async call chains, including EF Core calls.
- Keep configuration and secrets out of source control; use `IOptions<T>` for strongly typed configuration.

## Standard Structure

```text
MyProject.Api/
├── Domain/
│   ├── Entities/
│   └── Exceptions/
├── Features/
│   └── Products/
│       ├── CreateProduct/
│       │   ├── CreateProductEndpoint.cs
│       │   ├── CreateProductCommand.cs
│       │   ├── CreateProductValidator.cs
│       │   └── CreateProductHandler.cs
│       └── GetProductById/
│           ├── GetProductByIdEndpoint.cs
│           ├── GetProductByIdQuery.cs
│           └── GetProductByIdHandler.cs
├── Infrastructure/
│   └── Data/
│       └── AppDbContext.cs
└── Program.cs
```

## Feature Implementation

### Write Slices

For a create or update operation:

- Add a Minimal API or FastEndpoints endpoint in the feature folder.
- Define a MediatR command and a feature-specific response record.
- Add a FluentValidation validator for all input rules.
- Inject `AppDbContext` directly into the command handler.
- Persist changes asynchronously and return the feature-specific response.
- Use `decimal` for monetary values and apply explicit, currency-aware validation where applicable.

### Read Slices

For a read operation:

- Define a MediatR query and a feature-specific response record.
- Inject `AppDbContext` directly into the query handler.
- Use `AsNoTracking()` for read-only queries.
- Project explicitly with LINQ `.Select()` instead of loading unnecessary entities.
- Return an appropriate not-found result when the requested resource does not exist.

### Endpoints and Validation

- Prefer ASP.NET Core Minimal APIs or the repository's established endpoint framework over controllers with many actions.
- Keep endpoints responsible for HTTP concerns and handlers responsible for feature behavior.
- Validate input before executing handler logic; do not put manual validation blocks in handlers.
- Use explicit, localized request and response records. Do not reuse DTOs across unrelated endpoints.
- Preserve existing public API contracts unless a breaking change is explicitly required.

## Data Access and Mapping

- Use direct `DbContext` access in each handler; do not introduce `IProductRepository`, generic repositories, or boilerplate persistence interfaces.
- Use asynchronous EF Core operations such as `ToListAsync`, `SingleOrDefaultAsync`, and `SaveChangesAsync`.
- Use explicit projection or manual mapping inside the handler. Avoid reflection-based auto-mappers across feature boundaries.
- Keep transaction boundaries aligned with the feature operation and aggregate consistency requirements.
- Add indexes or caching only when supported by the feature's query and performance needs.

## Cross-Cutting Concerns

- Register MediatR, FluentValidation, the `DbContext`, and endpoint infrastructure in `Program.cs` using dependency injection.
- Apply authorization at the endpoint and resource/aggregate boundary where required.
- Use consistent error handling and problem-details responses.
- Capture significant financial state changes in an audit trail or domain event when the existing application supports those mechanisms.
- Do not add domain events, repositories, or additional layers solely to satisfy a generic architecture template; introduce them only when the business or infrastructure boundary requires them.

## Testing

- Test each slice through its public behavior, including validation, authorization, not-found, success, and persistence paths as applicable.
- Keep unit tests focused on feature-specific rules and integration tests focused on HTTP and database behavior.
- Name tests using `MethodName_Condition_ExpectedResult()` when that convention is used by the project.
- For financial features, cover decimal precision, currency handling, rounding, duplicate requests, and transaction integrity.

## Review Checklist

Before completing an implementation, verify:

- The code is organized by business capability and the slice is self-contained.
- Commands and queries are separated and routed through MediatR.
- Handlers use direct `DbContext` access without generic repository abstractions.
- Read queries use `AsNoTracking()` and explicit projection.
- Input validation is localized to the feature and occurs before handler execution.
- Endpoints expose the intended HTTP contract, authorization, and error responses.
- Async I/O, dependency injection, and modern C# practices are used consistently.
- Tests cover the changed slice and relevant edge cases.