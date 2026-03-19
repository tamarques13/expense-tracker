using Spentir.DTOs;
using Spentir.Models;

namespace Spentir.Domain.Models
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