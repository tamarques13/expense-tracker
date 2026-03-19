using Spentir.Domain.Models;
using Spentir.Models;
using Spentir.DTOs;

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
