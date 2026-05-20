using ExpenseTracker.Application.DTOs.Analytics;
using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Domain.Models.ValueObjects;

namespace ExpenseTracker.Domain.Services.Interfaces
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
