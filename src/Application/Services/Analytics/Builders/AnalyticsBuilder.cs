using Spentir.Application.Services.Analytics.Interfaces;
using Spentir.Application.DTOs.Analytics;
using Spentir.Application.Mappers.Analytics;
using Spentir.Domain.Models.ValueObjects;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Services.Interfaces;

namespace Spentir.Application.Services.Analytics.Builders
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
