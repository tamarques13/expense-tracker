using Spentir.Application.DTOs.Analytics;

namespace Spentir.Domain.Models.ValueObjects
{
    public record YearAnalyticsMetrics(
        decimal Total,
        decimal AverageMonthlySpent,
        decimal HighestSpendingAmount,
        decimal LowestSpendingAmount,
        string HighestSpendingCategory,
        string LowestSpendingCategory,
        List<CategoryYearAnalyticsDto> Categories,
        List<YearMonthsAnalyticsDto> Months
    );
}