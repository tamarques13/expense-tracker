using Spentir.Application.DTOs.Analytics;
using Spentir.Domain.Models.ValueObjects;
using Spentir.Domain.Models.Entities;

namespace Spentir.Application.Services.Analytics.Interfaces
{
    /// <summary>
    /// Builds analytics.
    /// </summary>
    public interface IAnalyticsBuilder
    {
        List<CategoryAnalyticsDto> BuildCategoryAnalytics(IDictionary<ExpenseCategory, CategoryAggregate> current, IDictionary<ExpenseCategory, decimal> previous, decimal totalSpent);
        List<YearMonthsAnalyticsDto> BuildMonthlyAnalytics((DateOnly Start, DateOnly End) range, int months, decimal totalSpent, Dictionary<(int Year, int Month), decimal> expensesGroup);
    }
}
