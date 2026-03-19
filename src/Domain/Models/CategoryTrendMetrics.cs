namespace Spentir.Domain.Models
{
    public record CategoryTrendMetrics(
        decimal CurrMonthAmount,
        decimal OldestMonthAmount,
        decimal TotalSpent,
        decimal TotalCategorySpent,
        decimal HighestAmount,
        decimal LowestAmount,
        decimal AverageMonthlySpent,
        decimal PercentageChange,
        decimal Multiplier,
        bool IsImproving
    );
}
 