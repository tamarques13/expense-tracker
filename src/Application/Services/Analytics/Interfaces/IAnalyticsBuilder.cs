using ExpenseTracker.Application.DTOs.Analytics;
using ExpenseTracker.Domain.Models.ValueObjects;
using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.Application.Services.Analytics.Interfaces
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
