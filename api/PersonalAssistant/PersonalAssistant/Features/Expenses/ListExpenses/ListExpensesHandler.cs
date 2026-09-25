using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalAssistant.Infrastructure.Data;

namespace PersonalAssistant.Features.Expenses.ListExpenses;

public class ListExpensesHandler(PersonalAssistantDbContext dbContext)
    : IRequestHandler<ListExpensesQuery, IReadOnlyList<ExpenseResponse>>
{
    public async Task<IReadOnlyList<ExpenseResponse>> Handle(ListExpensesQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Expenses
            .AsNoTracking()
            .OrderByDescending(e => e.Date)
            .Select(e => new ExpenseResponse(e.Id, e.Date, e.Value, e.Description, e.Category))
            .ToListAsync(cancellationToken);
    }
}
