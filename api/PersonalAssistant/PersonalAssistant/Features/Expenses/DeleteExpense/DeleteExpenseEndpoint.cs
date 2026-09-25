using MediatR;

namespace PersonalAssistant.Features.Expenses.DeleteExpense;

public static class DeleteExpenseEndpoint
{
    public static void MapDeleteExpense(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:int}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var deleted = await sender.Send(new DeleteExpenseCommand(id), cancellationToken);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteExpense")
            .WithOpenApi();
    }
}
