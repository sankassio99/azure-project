namespace PersonalAssistant.Features.Expenses;

public record ExpenseResponse(int Id, DateOnly Date, decimal Value, string Description, string Category);
