using Spentir.Domain.Models;
using Spentir.Domain.Services.Interfaces;
using Spentir.DTOs;
using Spentir.Models;

namespace Spentir.Domain.Services
{
    public class MonthAnalyticsCalculator : IMonthAnalyticsCalculator
    {
        public MonthAnalyticsMetrics Calculate(DateOnly date, List<Expense> expenses, decimal totalSpent, List<CategoryAnalyticsDto> categories, TrendAnalyticsDto trend)
        {
            var sortedAmounts = expenses.Select(e => e.Amount).OrderBy(x => x).ToList();
            var median = sortedAmounts.Count == 0 ? 0 : sortedAmounts[sortedAmounts.Count / 2];

            var highest = expenses.OrderByDescending(e => e.Amount).FirstOrDefault();
            var average = Math.Round(totalSpent / DateTime.DaysInMonth(date.Year, date.Month), 2);

            return new MonthAnalyticsMetrics(
                Total: totalSpent,
                AverageDailySpent: average,
                MedianExpense: median,
                LargestExpense: highest?.Amount ?? 0,
                LargestExpenseCategoryName: highest?.Category.ToString() ?? "N/A",
                LargestExpenseCategory: highest?.Category,
                Categories: categories,
                Trend: trend
            );
        }
    }
}