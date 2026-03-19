using Spentir.Application.DTOs.Analytics;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Domain.Services.Interfaces
{
    public interface IMonthAnalyticsCalculator
    {
        MonthAnalyticsMetrics Calculate(
            DateOnly date,
            List<Expense> expenses,
            Dictionary<ExpenseCategory, CategoryAggregate> grouped,
            decimal totalSpent,
            List<CategoryAnalyticsDto> categories,
            IEnumerable<Expense> previous
        );
    }
}
