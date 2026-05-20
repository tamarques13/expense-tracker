using ExpenseTracker.Domain.Services.Interfaces;
using ExpenseTracker.Application.DTOs.Analytics;
using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Domain.Models.ValueObjects;

namespace ExpenseTracker.Domain.Services
{
    /// <summary>
    /// Domain service responsible for computing yearly analytics metrics based on
    /// category aggregates, total spending and month‑by‑month summaries. Produces
    /// values such as highest and lowest spending categories, average monthly
    /// spending and the full yearly analytics breakdown.
    /// </summary>

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