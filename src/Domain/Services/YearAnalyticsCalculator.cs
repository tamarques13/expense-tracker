using Spentir.Domain.Services.Interfaces;
using Spentir.Application.DTOs.Analytics;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Domain.Services
{
    public class YearAnalyticsCalculator : IYearAnalyticsCalculator
    {
        public YearAnalyticsMetrics Calculate(Dictionary<ExpenseCategory, CategoryAggregate> grouped, decimal totalSpent, List<CategoryYearAnalyticsDto> categories, List<YearMonthsAnalyticsDto> monthly)
        {
            var highest = grouped.OrderByDescending(e => e.Value.Total).FirstOrDefault();
            var lowest = grouped.OrderBy(e => e.Value.Total).FirstOrDefault();
            var average = Math.Round(totalSpent / 12, 2);

            return new YearAnalyticsMetrics(
                Total: totalSpent,
                AverageMonthlySpent: average,
                HighestSpendingAmount: highest.Value.Total,
                LowestSpendingAmount: lowest.Value.Total,
                HighestSpendingCategory: highest.Key.ToString() ?? "N/A",
                LowestSpendingCategory: lowest.Key.ToString() ?? "N/A",
                Categories: categories,
                Months: monthly
            );
        }
    }
}