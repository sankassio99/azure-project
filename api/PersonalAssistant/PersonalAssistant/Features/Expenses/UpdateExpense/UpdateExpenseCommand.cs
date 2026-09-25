using MediatR;

namespace PersonalAssistant.Features.Expenses.UpdateExpense;

public record UpdateExpenseCommand(int Id, DateOnly Date, decimal Value, string Description, string Category)
    : IRequest<ExpenseResponse?>;
