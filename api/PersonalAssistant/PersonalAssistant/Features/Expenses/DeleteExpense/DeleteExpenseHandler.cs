using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalAssistant.Infrastructure.Data;

namespace PersonalAssistant.Features.Expenses.DeleteExpense;

public class DeleteExpenseHandler(PersonalAssistantDbContext dbContext) : IRequestHandler<DeleteExpenseCommand, bool>
{
    public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await dbContext.Expenses.SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        if (expense is null)
        {
            return false;
        }

        dbContext.Expenses.Remove(expense);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
