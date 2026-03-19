using Spentir.Application.DTOs;
using Spentir.Domain.Models.Entities;

namespace Spentir.Domain.Models.ValueObjects
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
