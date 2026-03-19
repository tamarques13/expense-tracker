using Spentir.DTOs;
using Spentir.Models;

namespace Spentir.Domain.Models
{
    public record TrendAnalyticsMetrics(
        decimal PreviousTotal,
        decimal Change,
        decimal MonthChange
    );
}
