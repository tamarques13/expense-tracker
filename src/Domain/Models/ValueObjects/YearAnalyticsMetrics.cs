using Spentir.Application.DTOs;

namespace Spentir.Domain.Models.ValueObjects
{
    public record YearAnalyticsMetrics(
        decimal Total,
        decimal AverageMonthlySpent,
        string HighestSpendingCategory,
        decimal HighestSpendingAmount,
        string LowestSpendingCategory,
        decimal LowestSpendingAmount,
        List<CategoryYearAnalyticsDto> Categories,
        List<YearMonthsAnalyticsDto> Months
    );
}