using ExpenseTracker.Application.DTOs.Analytics;
using ExpenseTracker.Domain.Models.ValueObjects;

namespace ExpenseTracker.Application.Mappers.Analytics
{
    public class MonthAnalyticsDtoBuilder
    {
        /// <summary>
        /// Constructs the final monthly analytics summary, including global statistics such as total
        /// spending, average daily spending, median expense and largest expense. Attaches category-level
        /// analytics and trend metrics to produce a complete <see cref="MonthAnalyticsDto"/>.
        /// </summary>
        /// <param name="date">The reporting month.</param>
        /// <param name="metrics">The fully computed month metrics for the selected date, produced by the domain service.</param>

        public static MonthAnalyticsDto Build(MonthAnalyticsMetrics metrics, DateOnly date)
        {
            return new MonthAnalyticsDto
            {
                Year = date.Year,
                Month = date.Month,
                Amounts = new MonthAmountsDto
                {
                    Total = metrics.Total,
                    AverageDaily = metrics.AverageDailySpent,
                    Median = metrics.MedianExpense,
                    Largest = metrics.LargestExpense,
                },
                SpendingCategory = new SpendingCategory
                {
                    Highest = metrics.HighestExpenseCategoryName,
                    Lowest = metrics.LowestExpenseCategoryName
                },
                Categories = metrics.Categories,
                Trend = new TrendAnalyticsDto
                {
                    PreviousMonthTotal = metrics.PreviousTotal,
                    MonthChange = metrics.Change,
                    MonthPercentageChange = metrics.MonthChange,
                    Multiplier = metrics.Multiplier,
                    IsImproving = metrics.IsImproving
                }
            };
        }
    }
}