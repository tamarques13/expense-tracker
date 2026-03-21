using Spentir.Domain.Models.Entities;

namespace Spentir.Application.DTOs.Analytics
{
    public class CategoryTrendDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public ExpenseCategory Category { get; set; }
        public PeriodDto Period { get; set; } = new();
        public CategoryTrendAmountsDto Amounts { get; set; } = new();
        public CategoryTrendMetricsDto Metrics { get; set; } = new();
    }

    public class PeriodDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class CategoryTrendAmountsDto
    {
        public decimal Current { get; set; }
        public decimal Average { get; set; }
        public decimal Total { get; set; }
        public decimal Highest { get; set; }
        public decimal Lowest { get; set; }
    }

    public class CategoryTrendMetricsDto
    {
        public decimal PercentageChange { get; set; }
        public decimal Multiplier { get; set; }
        public bool IsImproving { get; set; }
    }
}