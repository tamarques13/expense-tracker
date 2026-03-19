using Spentir.Application.DTOs;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Application.Mappers
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
                Total = metrics.Total,
                AverageMonthlySpent = metrics.AverageMonthlySpent,
                HighestSpendingCategory = metrics.HighestSpendingCategory,
                HighestSpendingAmount = metrics.HighestSpendingAmount,
                LowestSpendingCategory = metrics.LowestSpendingCategory,
                LowestSpendingAmount = metrics.LowestSpendingAmount,
                Categories = metrics.Categories,
                Months = metrics.Months
            };
        }
    }
}