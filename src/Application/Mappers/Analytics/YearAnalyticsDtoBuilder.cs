using ExpenseTracker.Application.DTOs.Analytics;
using ExpenseTracker.Domain.Models.ValueObjects;

namespace ExpenseTracker.Application.Mappers.Analytics
{
    public class YearAnalyticsDtoBuilder
    {
        /// <summary>
        /// Constructs the final yearly analytics summary, including global statistics such as total
        /// spending, average category spending, highest and lowest spending category. .
        /// </summary>
        /// <param name="date">The month and year for which the summary is being generated.</param>
        /// <param name="metrics">The fully computed month metrics for the selected Year, produced by the domain service.</param>


        public static YearAnalyticsDto Build(DateOnly date, YearAnalyticsMetrics metrics)
        {
            return new YearAnalyticsDto
            {
                Year = date.Year,
                Amounts = new YearAmountsDto
                {
                    Total = metrics.Total,
                    Average = metrics.AverageMonthlySpent,
                    Highest = metrics.HighestSpendingAmount,
                    Lowest = metrics.LowestSpendingAmount
                },
                SpendingCategory = new SpendingCategory
                {
                    Highest = metrics.HighestSpendingCategory,
                    Lowest =  metrics.LowestSpendingCategory
                },
                Categories = metrics.Categories,
                Months = metrics.Months
            };
        }
    }
}