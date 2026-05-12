using ExpenseTracker.Domain.Services.Interfaces;
using ExpenseTracker.Application.DTOs.Analytics;
using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Domain.Models.ValueObjects;

namespace ExpenseTracker.Domain.Services
{
    /// <summary>
    /// Domain service responsible for calculating spending trends for a specific
    /// expense category over a multi month period. Computes metrics such as
    /// highest and lowest amounts, percentage change, multipliers and overall
    /// trend direction based on aggregated monthly analytics.
    /// </summary>

    public class CategoryTrendCalculator : ICategoryTrendCalculator
    {
        public CategoryTrendMetrics Calculate(List<Expense> expenses, ExpenseCategory category, List<YearMonthsAnalyticsDto> monthlyTotals, decimal totalCategorySpent, decimal totalSpent)
        {
            var catExpenses = expenses.Where(e => e.Category == category).ToList();

            var highest = catExpenses.OrderByDescending(e => e.Amount).FirstOrDefault()?.Amount ?? 0;
            var lowest = catExpenses.OrderBy(e => e.Amount).FirstOrDefault()?.Amount ?? 0;

            var currMonth = monthlyTotals.Last().Total;
            var targetMonth = monthlyTotals.First().Total;

            decimal percentageChange;

            if (targetMonth == 0)
                percentageChange = currMonth == 0 ? 0 : 100;
            else
                percentageChange = Math.Round((currMonth - targetMonth) / targetMonth * 100, 2);

            var multiplier = Math.Round(1 + (percentageChange / 100m), 2);
            var avgMonthly = Math.Round(totalCategorySpent / monthlyTotals.Count, 2);

            return new CategoryTrendMetrics(
                TargetYear: monthlyTotals.First().Year,
                TargetMonth: monthlyTotals.First().Month,
                CurrMonthAmount: currMonth,
                TotalSpent: totalSpent,
                TotalCategorySpent: totalCategorySpent,
                HighestAmount: highest,
                LowestAmount: lowest,
                AverageMonthlySpent: avgMonthly,
                PercentageChange: percentageChange,
                Multiplier: multiplier,
                IsImproving: percentageChange < 0
            );
        }
    }
}
