namespace ExpenseTracker.Domain.Models.ValueObjects
{
    public record CategoryTrendMetrics(
        int TargetYear,
        int TargetMonth,
        decimal CurrMonthAmount,
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
