using Spentir.Domain.Services.Interfaces;
using Spentir.Application.DTOs.Analytics;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Domain.Services
{
    public class MonthAnalyticsCalculator : IMonthAnalyticsCalculator
    {
        public MonthAnalyticsMetrics Calculate(DateOnly date, List<Expense> expenses, Dictionary<ExpenseCategory, CategoryAggregate> grouped, decimal totalSpent, List<CategoryAnalyticsDto> categories, IEnumerable<Expense> current, IEnumerable<Expense> previous)
        {
            var sortedAmounts = expenses.Select(e => e.Amount).OrderBy(x => x).ToList();
            var median = sortedAmounts.Count == 0 ? 0 : sortedAmounts[sortedAmounts.Count / 2];
            var largestExpense = expenses.OrderByDescending(e => e.Amount).FirstOrDefault();

            var highest = grouped.OrderByDescending(e => e.Value.Total).FirstOrDefault();
            var lowest = grouped.OrderBy(e => e.Value.Total).FirstOrDefault();
            var average = Math.Round(totalSpent / DateTime.DaysInMonth(date.Year, date.Month), 2);

            var currentTotal = current.Sum(e => e.Amount);
            var previousTotal = previous.Sum(e => e.Amount);

            var change = currentTotal - previousTotal;

            var monthChange = previousTotal == 0 ? 0 : Math.Round(change / previousTotal * 100, 2);
            var multiplier = Math.Round(1 + (monthChange / 100m), 2);

            return new MonthAnalyticsMetrics(
                Total: totalSpent,
                AverageDailySpent: average,
                MedianExpense: median,
                LargestExpense: largestExpense?.Amount ?? 0,
                HighestExpenseCategoryName: highest.Key.ToString() ?? "N/A",
                LowestExpenseCategoryName: lowest.Key.ToString() ?? "N/A",
                Categories: categories,
                PreviousTotal: previousTotal,
                Change: change,
                MonthChange: monthChange,
                Multiplier: multiplier,
                IsImproving: change < 0
            );
        }
    }
}