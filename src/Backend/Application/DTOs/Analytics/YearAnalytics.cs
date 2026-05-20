using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.Application.DTOs.Analytics
{
    public class YearAnalyticsDto
    {
        public int Year { get; set; }
        public YearAmountsDto Amounts { get; set; } = new();
        public SpendingCategory SpendingCategory { get; set; } = new();
        public List<CategoryYearAnalyticsDto> Categories { get; set; } = new();
        public List<YearMonthsAnalyticsDto> Months { get; set; } = new();
    }

    public class YearAmountsDto
    {
        public decimal Total { get; set; }
        public decimal Average { get; set; }
        public decimal Highest { get; set; }
        public decimal Lowest { get; set; }
    }

    public class CategoryYearAnalyticsDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public ExpenseCategory Category { get; set; }
        public decimal Total { get; set; }
        public decimal Percentage { get; set; }
    }

    public class YearMonthsAnalyticsDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Total { get; set; }
        public decimal Percentage { get; set; }
    }
}