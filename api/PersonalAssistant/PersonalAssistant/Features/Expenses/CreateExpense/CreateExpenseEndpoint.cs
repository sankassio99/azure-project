using MediatR;

namespace PersonalAssistant.Features.Expenses.CreateExpense;

public static class CreateExpenseEndpoint
{
    public static void MapCreateExpense(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (CreateExpenseCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var expense = await sender.Send(command, cancellationToken);
                return Results.Created($"/api/expenses/{expense.Id}", expense);
            })
            .WithName("CreateExpense")
            .WithOpenApi();
    }
}
