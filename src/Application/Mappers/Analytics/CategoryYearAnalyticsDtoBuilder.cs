using ExpenseTracker.Domain.Models.ValueObjects;
using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Application.DTOs.Analytics;

namespace ExpenseTracker.Application.Mappers.Analytics
{
    public class CategoryYearAnalyticsDtoBuilder
    {
        /// <summary>
        /// Builds a complete set of annual category analytics DTOs. Ensures that all categories
        /// defined in <see cref="ExpenseCategory"/> are included, even if no expenses were recorded
        /// for a given category during the year. Calculates total spending and each category’s
        /// percentage contribution to the yearly total.
        /// </summary>
        /// <param name="grouped">The aggregated yearly metrics per category, containing total and average values.</param>
        /// <param name="totalSpent">The total amount spent across all categories during the year.</param>

        public static List<CategoryYearAnalyticsDto> Build(Dictionary<ExpenseCategory, CategoryAggregate> grouped, decimal totalSpent)
        {
            return Enum.GetValues<ExpenseCategory>().Select(c =>
                {
                    grouped.TryGetValue(c, out var data);

                    var total = data?.Total ?? 0;

                    return new CategoryYearAnalyticsDto
                    {
                        CategoryName = c.ToString(),
                        Category = c,
                        Total = total,
                        Percentage = totalSpent == 0 ? 0 : Math.Round(total / totalSpent * 100, 2)
                    };
                }).ToList();
        }
    }
}