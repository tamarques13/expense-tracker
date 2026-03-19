using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Domain.Services.Interfaces
{
    public interface ITrendAnalyticsCalculator
    {
        TrendAnalyticsMetrics Calculate(
            IEnumerable<Expense> current,
            IEnumerable<Expense> previous
        );
    }
}
