using Spentir.Domain.Models;
using Spentir.Models;
using Spentir.DTOs;

namespace Spentir.Helpers.Builders
{
    public class CategoryAnalyticsDtoBuilder
    {
        /// <summary>
        /// Constructs a complete set of category analytics DTOs. Ensures that all categories defined
        /// in <see cref="ExpenseCategory"/> are represented, even if no expenses exist for a category
        /// in the current month. Computes totals, averages, transaction counts and percentage-of-total
        /// metrics for each category.
        /// </summary>
        /// <param name="grouped">The aggregated category metrics for the current month.</param>
        /// <param name="totalSpent">The total amount spent in the current month.</param>

        public static List<CategoryAnalyticsDto> Build(Dictionary<ExpenseCategory, CategoryAggregate> grouped, decimal totalSpent)
        {
            return Enum.GetValues<ExpenseCategory>().Select(c =>
                {
                    grouped.TryGetValue(c, out var data);

                    var total = data?.Total ?? 0;

                    return new CategoryAnalyticsDto
                    {
                        CategoryName = c.ToString(),
                        Category = c,
                        Total = total,
                        TransationsNum = data?.Transactions ?? 0,
                        Average = data?.Average ?? 0,
                        Percentage = totalSpent == 0 ? 0 : Math.Round(total / totalSpent * 100, 2)
                    };
                }).ToList();
        }

        /// <summary>
        /// Enhances each category analytics DTO with previous month totals and computes the absolute
        /// month-over-month change. This enables comparative insights across reporting periods.
        /// </summary>
        /// <param name="categories">The category DTOs to enrich with comparison data.</param>
        /// <param name="previousExpenses">The expenses from the previous month.</param>

        public static void ApplyPreviousMonthComparison(List<CategoryAnalyticsDto> categories, Dictionary<ExpenseCategory, decimal> groupedPrev)
        {
            foreach (var c in categories)
            {
                groupedPrev.TryGetValue(c.Category, out var prevTotal);
                c.PreviousMonthTotal = prevTotal;
                c.LastMonthChange = c.Total - prevTotal;
            }
        }
    }
}