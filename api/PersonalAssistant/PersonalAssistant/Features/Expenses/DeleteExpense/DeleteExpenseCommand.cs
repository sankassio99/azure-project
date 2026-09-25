using MediatR;

namespace PersonalAssistant.Features.Expenses.DeleteExpense;

public record DeleteExpenseCommand(int Id) : IRequest<bool>;
