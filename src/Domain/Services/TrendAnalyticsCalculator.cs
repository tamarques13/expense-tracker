using Spentir.Domain.Services.Interfaces;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Domain.Services
{
    public class TrendAnalyticsCalculator : ITrendAnalyticsCalculator
    {
        public TrendAnalyticsMetrics Calculate(IEnumerable<Expense> current, IEnumerable<Expense> previous)
        {
            var currentTotal = current.Sum(e => e.Amount);
            var previousTotal = previous.Sum(e => e.Amount);

            var change = currentTotal - previousTotal;

            var monthChange = previousTotal == 0 ? 0 : Math.Round(change / previousTotal * 100, 2);

            return new TrendAnalyticsMetrics(
                PreviousTotal: previousTotal,
                Change: change,
                MonthChange: monthChange
            );
        }
    }
}