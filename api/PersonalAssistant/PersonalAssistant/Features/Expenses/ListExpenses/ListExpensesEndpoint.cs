using MediatR;

namespace PersonalAssistant.Features.Expenses.ListExpenses;

public static class ListExpensesEndpoint
{
    public static void MapListExpenses(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var expenses = await sender.Send(new ListExpensesQuery(), cancellationToken);
                return Results.Ok(expenses);
            })
            .WithName("ListExpenses")
            .WithOpenApi();
    }
}
