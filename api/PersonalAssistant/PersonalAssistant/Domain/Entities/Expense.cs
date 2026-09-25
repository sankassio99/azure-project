namespace PersonalAssistant.Domain.Entities;

public class Expense
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public decimal Value { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
}
