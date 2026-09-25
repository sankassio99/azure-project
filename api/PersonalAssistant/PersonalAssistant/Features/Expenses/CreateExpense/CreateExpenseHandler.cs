using MediatR;
using PersonalAssistant.Domain.Entities;
using PersonalAssistant.Infrastructure.Data;

namespace PersonalAssistant.Features.Expenses.CreateExpense;

public class CreateExpenseHandler(PersonalAssistantDbContext dbContext)
    : IRequestHandler<CreateExpenseCommand, ExpenseResponse>
{
    public async Task<ExpenseResponse> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            Date = request.Date,
            Value = request.Value,
            Description = request.Description,
            Category = request.Category
        };

        dbContext.Expenses.Add(expense);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ExpenseResponse(expense.Id, expense.Date, expense.Value, expense.Description, expense.Category);
    }
}
