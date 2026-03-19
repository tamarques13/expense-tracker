using Spentir.Domain.Models;
using Spentir.DTOs;
using Spentir.Models;

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
