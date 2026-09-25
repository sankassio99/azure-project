using MediatR;

namespace PersonalAssistant.Features.Expenses.CreateExpense;

public record CreateExpenseCommand(DateOnly Date, decimal Value, string Description, string Category)
    : IRequest<ExpenseResponse>;
