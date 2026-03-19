using Spentir.Domain.Models;
using Spentir.Models;
using Spentir.DTOs;

namespace Spentir.Helpers.Builders
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
                CurrMonthAmount = metrics.CurrMonthAmount,
                AverageMonthlySpent = metrics.AverageMonthlySpent,
                TotalSpent = metrics.TotalCategorySpent,
                HighestAmount = metrics.HighestAmount,
                LowestAmount = metrics.LowestAmount,
                PercentageChange = metrics.PercentageChange,
                Multiplier = metrics.Multiplier,
                IsImproving = metrics.IsImproving
            };
        }
    }
}