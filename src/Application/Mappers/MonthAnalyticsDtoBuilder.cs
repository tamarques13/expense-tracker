using Spentir.Application.DTOs;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Application.Mappers
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
                Total = metrics.Total,
                AverageDailySpent = metrics.AverageDailySpent,
                MedianExpense = metrics.MedianExpense,
                LargestExpense = metrics.LargestExpense,
                LargestExpenseCategoryName = metrics.LargestExpenseCategoryName,
                LargestExpenseCategory = metrics.LargestExpenseCategory,
                Categories = metrics.Categories,
                Trend = metrics.Trend
            };
        }
    }
}