using Spentir.Application.DTOs.Analytics;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Domain.Services.Interfaces
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
