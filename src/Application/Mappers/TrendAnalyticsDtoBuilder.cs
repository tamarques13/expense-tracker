using Spentir.Application.DTOs;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Application.Mappers
{
    public class TrendAnalyticsDtoBuilder
    {
        /// <summary>
        /// Computes high-level spending trends between the current and previous month, including
        /// total previous-month spending, absolute change and percentage change. These metrics
        /// support summary trend visualizations and financial insights.
        /// </summary>
        /// <param name="metrics">The fully computed month metrics for the selected Trend, produced by the domain service.</param>

        public static TrendAnalyticsDto Build(TrendAnalyticsMetrics metrics)
        {
             return new TrendAnalyticsDto
            {
                PreviousMonthTotal = metrics.PreviousTotal,
                LastMonthChange = metrics.Change,
                MonthPercentageChange = metrics.MonthChange
            };
        }
    }
}