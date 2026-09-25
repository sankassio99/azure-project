using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalAssistant.Infrastructure.Data;

namespace PersonalAssistant.Features.Expenses.UpdateExpense;

public class UpdateExpenseHandler(PersonalAssistantDbContext dbContext)
    : IRequestHandler<UpdateExpenseCommand, ExpenseResponse?>
{
    public async Task<ExpenseResponse?> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await dbContext.Expenses.SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        if (expense is null)
        {
            return null;
        }

        expense.Date = request.Date;
        expense.Value = request.Value;
        expense.Description = request.Description;
        expense.Category = request.Category;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new ExpenseResponse(expense.Id, expense.Date, expense.Value, expense.Description, expense.Category);
    }
}
