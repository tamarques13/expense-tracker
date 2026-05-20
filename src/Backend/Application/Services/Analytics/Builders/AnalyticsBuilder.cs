using ExpenseTracker.Application.Services.Analytics.Interfaces;
using ExpenseTracker.Application.DTOs.Analytics;
using ExpenseTracker.Application.Mappers.Analytics;
using ExpenseTracker.Domain.Models.ValueObjects;
using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Domain.Services.Interfaces;

namespace ExpenseTracker.Application.Services.Analytics.Builders
{
    /// <summary>
    /// Builds category level analytics.
    /// </summary>
    public class AnalyticsBuilder(IDateRangeService dateRangeService) : IAnalyticsBuilder
    {
        private readonly IDateRangeService _dateRangeService = dateRangeService;

        public List<CategoryAnalyticsDto> BuildCategoryAnalytics(IDictionary<ExpenseCategory, CategoryAggregate> current, IDictionary<ExpenseCategory, decimal> previous, decimal totalSpent)
        {
            var categories = CategoryAnalyticsDtoBuilder.Build(current, totalSpent);
            CategoryAnalyticsDtoBuilder.ApplyPreviousMonthComparison(categories, previous);

            return categories;
        }

        public List<YearMonthsAnalyticsDto> BuildMonthlyAnalytics((DateOnly Start, DateOnly End) range, int months, decimal totalSpent, Dictionary<(int Year, int Month), decimal> expensesGroup)
        {
            var rollingMonths = _dateRangeService.GetRollingMonths(range.Start, months);

            return MonthlyAnalyticsDtoBuilder.Build(totalSpent, expensesGroup, rollingMonths);
        }
    }
}
