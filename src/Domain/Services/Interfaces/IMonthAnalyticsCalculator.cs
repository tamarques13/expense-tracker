using Spentir.Application.DTOs;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

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
