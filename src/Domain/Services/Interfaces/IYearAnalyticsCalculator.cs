using Spentir.Application.DTOs.Analytics;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Domain.Services.Interfaces
{
    public interface IYearAnalyticsCalculator
    {
        YearAnalyticsMetrics Calculate(
            Dictionary<ExpenseCategory, CategoryAggregate> grouped,
            decimal totalSpent,
            List<CategoryYearAnalyticsDto> categories,
            List<YearMonthsAnalyticsDto> monthly
        );
    }
}
