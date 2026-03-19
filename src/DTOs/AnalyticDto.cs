using Spentir.Models;

namespace Spentir.DTOs
{
    public class MonthAnalyticsDto
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal Total { get; set; }
        public decimal AverageDailySpent { get; set; }
        public decimal MedianExpense { get; set; }
        public decimal LargestExpense { get; set; }
        public string LargestExpenseCategoryName { get; set; } = string.Empty;
        public ExpenseCategory? LargestExpenseCategory { get; set; }
        public List<CategoryAnalyticsDto> Categories { get; set; } = new();
        public TrendAnalyticsDto? Trend { get; set; }
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

    public class TrendAnalyticsDto
    {
        public decimal PreviousMonthTotal { get; set; }
        public decimal LastMonthChange { get; set; }
        public decimal MonthPercentageChange { get; set; }
    }

    public class YearAnalyticsDto
    {
        public int Year { get; set; }
        public decimal Total { get; set; }
        public decimal AverageMonthlySpent { get; set; }
        public string HighestSpendingCategory { get; set; } = string.Empty;
        public decimal HighestSpendingAmount { get; set; }
        public string LowestSpendingCategory { get; set; } = string.Empty;
        public decimal LowestSpendingAmount { get; set; }
        public List<CategoryYearAnalyticsDto> Categories { get; set; } = new();
        public List<YearMonthsAnalyticsDto> Months { get; set; } = new();
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

    public class CategoryTrendDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public ExpenseCategory Category { get; set; }
        public decimal CurrMonthAmount { get; set; }
        public decimal AverageMonthlySpent { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal HighestAmount { get; set; }
        public decimal LowestAmount { get; set; }
        public decimal PercentageChange { get; set; }
        public decimal Multiplier {get; set;}
        public bool IsImproving { get; set; }
    }
}