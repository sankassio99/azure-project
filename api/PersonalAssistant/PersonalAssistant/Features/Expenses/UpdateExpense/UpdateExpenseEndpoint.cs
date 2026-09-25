using MediatR;

namespace PersonalAssistant.Features.Expenses.UpdateExpense;

public static class UpdateExpenseEndpoint
{
    public static void MapUpdateExpense(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}", async (int id, UpdateExpenseRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdateExpenseCommand(id, request.Date, request.Value, request.Description, request.Category);
                var expense = await sender.Send(command, cancellationToken);
                return expense is null ? Results.NotFound() : Results.Ok(expense);
            })
            .WithName("UpdateExpense")
            .WithOpenApi();
    }
}

public record UpdateExpenseRequest(DateOnly Date, decimal Value, string Description, string Category);
