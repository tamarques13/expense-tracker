using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.Application.DTOs.Analytics
{
    public class MonthAnalyticsDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public MonthAmountsDto Amounts { get; set; } = new();
        public SpendingCategory SpendingCategory { get; set; } = new();
        public TrendAnalyticsDto? Trend { get; set; }
        public List<CategoryAnalyticsDto> Categories { get; set; } = new();
    }

    public class MonthAmountsDto
    {
        public decimal Total { get; set; }
        public decimal AverageDaily { get; set; }
        public decimal Median { get; set; }
        public decimal Largest { get; set; }
    }

    public class TrendAnalyticsDto
    {
        public decimal? PreviousMonthTotal { get; set; }
        public decimal MonthChange { get; set; }
        public decimal MonthPercentageChange { get; set; }
        public decimal Multiplier { get; set; }
        public bool IsImproving { get; set; }
    }

    public class CategoryAnalyticsDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public ExpenseCategory Category { get; set; }
        public decimal Total { get; set; }
        public int TransationsNum { get; set; }
        public decimal Average { get; set; }
        public decimal Percentage { get; set; }
        public decimal? PreviousMonthTotal { get; set; }
        public decimal? LastMonthChange { get; set; }
    }

}