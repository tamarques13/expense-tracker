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

    public class ExpenseListDto
    {
        public List<ExpenseDto> Items { get; set; } = [];
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}