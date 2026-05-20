using ExpenseTracker.Application.DTOs.Analytics;
using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Domain.Models.ValueObjects;

namespace ExpenseTracker.Domain.Services.Interfaces
{
    public interface ICategoryTrendCalculator
    {
        CategoryTrendMetrics Calculate(
            List<Expense> expenses,
            ExpenseCategory category,
            List<YearMonthsAnalyticsDto> monthlyTotals,
            decimal totalCategorySpent,
            decimal totalSpent
        );
    }
}
