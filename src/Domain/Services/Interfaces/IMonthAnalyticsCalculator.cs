using Spentir.Domain.Models;
using Spentir.DTOs;
using Spentir.Models;

namespace Spentir.Domain.Services.Interfaces
{
    public interface IMonthAnalyticsCalculator
    {
        MonthAnalyticsMetrics Calculate(
            DateOnly date,
            List<Expense> expenses,
            decimal totalSpent,
            List<CategoryAnalyticsDto> categories,
            TrendAnalyticsDto trend
        );
    }
}
