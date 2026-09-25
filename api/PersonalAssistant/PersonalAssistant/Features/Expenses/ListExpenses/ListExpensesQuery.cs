using MediatR;

namespace PersonalAssistant.Features.Expenses.ListExpenses;

public record ListExpensesQuery : IRequest<IReadOnlyList<ExpenseResponse>>;
