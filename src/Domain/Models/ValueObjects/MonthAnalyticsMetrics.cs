using ExpenseTracker.Application.DTOs.Analytics;

namespace ExpenseTracker.Domain.Models.ValueObjects
{
    public record MonthAnalyticsMetrics(
        decimal Total,
        decimal AverageDailySpent,
        decimal MedianExpense,
        decimal LargestExpense,
        string HighestExpenseCategoryName,
        string LowestExpenseCategoryName,
        List<CategoryAnalyticsDto> Categories,
        decimal PreviousTotal,
        decimal Change,
        decimal MonthChange,
        decimal Multiplier,
        bool IsImproving
    );
}
