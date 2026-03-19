using Spentir.DTOs;
using Spentir.Models;

namespace Spentir.Domain.Models
{
    public record MonthAnalyticsMetrics(
        decimal Total,
        decimal AverageDailySpent,
        decimal MedianExpense,
        decimal LargestExpense,
        string LargestExpenseCategoryName,
        ExpenseCategory? LargestExpenseCategory,
        List<CategoryAnalyticsDto> Categories,
        TrendAnalyticsDto Trend
    );
}
