namespace Spentir.Application.DTOs
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly CreatedAt { get; set; }
    }

    public class CreateExpenseDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}