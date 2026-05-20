using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Domain.Models.ValueObjects;
using ExpenseTracker.Application.DTOs.Analytics;

namespace ExpenseTracker.Application.Mappers.Analytics
{
    public class CategoryTrendDtoBuilder
    {
        /// <summary>
        /// Maps the domain‑level trend metrics for a specific expense category into a
        /// presentation‑ready <see cref="CategoryTrendDto"/>. This method performs no business
        /// calculations; it simply transforms the already computed values from 
        /// <see cref="CategoryTrendMetrics"/> into the DTO structure consumed by the application layer.
        /// </summary>
        /// <param name="metrics">The fully computed trend metrics for the selected category, produced by the domain service.</param>
        /// <param name="category">The expense category to which the trend metrics apply.</param>

        public static CategoryTrendDto Build(CategoryTrendMetrics metrics, ExpenseCategory category)
        {
            return new CategoryTrendDto
            {
                CategoryName = category.ToString(),
                Category = category,
                Period = new PeriodDto
                {
                    Year = metrics.TargetYear,
                    Month = metrics.TargetMonth
                },
                Amounts = new CategoryTrendAmountsDto
                {
                    Current = metrics.CurrMonthAmount,
                    Average = metrics.AverageMonthlySpent,
                    Total = metrics.TotalCategorySpent,
                    Highest = metrics.HighestAmount,
                    Lowest = metrics.LowestAmount
                },
                Metrics = new CategoryTrendMetricsDto
                {
                    PercentageChange = metrics.PercentageChange,
                    Multiplier = metrics.Multiplier,
                    IsImproving = metrics.IsImproving
                }
            };
        }
    }
}