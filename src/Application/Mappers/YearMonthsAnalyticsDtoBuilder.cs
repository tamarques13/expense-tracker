using Spentir.Application.DTOs.Analytics;

namespace Spentir.Application.Mappers
{
    public class MonthlyAnalyticsDtoBuilder
    {
        /// <summary>
        /// Produces a month by month spending summary for a full year by grouping all expenses
        /// according to their creation month and aggregating the total amount spent in each period.
        /// </summary>
        /// <param name="totalSpent">The total amount spent across all categories during the month.</param>
        /// <param name="grouped">The aggregated category metrics for the current month.</param>
        /// <param name="months">The list of months to go through</param>

        public static List<YearMonthsAnalyticsDto> Build(decimal totalSpent, Dictionary<(int Year, int Month), decimal> grouped, IEnumerable<DateOnly> months)
        {
            var result = new List<YearMonthsAnalyticsDto>(12);

            foreach (var date in months)
            {
                grouped.TryGetValue((date.Year, date.Month), out var monthTotal);

                var dto = new YearMonthsAnalyticsDto
                {
                    Year = date.Year,
                    Month = date.Month,
                    Total = monthTotal,
                    Percentage = totalSpent == 0 ? 0 : Math.Round(monthTotal / totalSpent * 100, 2)
                };

                result.Add(dto);
            }

            return result;
        }
    }
}