using ExpenseTracker.Application.DTOs.Analytics;
using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Domain.Models.ValueObjects;

namespace ExpenseTracker.Domain.Services.Interfaces
{
    public interface IYearAnalyticsCalculator
    {
        YearAnalyticsMetrics Calculate(
            Dictionary<ExpenseCategory, CategoryAggregate> grouped,
            decimal totalSpent,
            List<CategoryYearAnalyticsDto> categories,
            List<YearMonthsAnalyticsDto> monthly
        );
    }
}
