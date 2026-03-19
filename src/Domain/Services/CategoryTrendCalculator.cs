using Spentir.Domain.Services.Interfaces;
using Spentir.Application.DTOs;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Domain.Services
{
    public class CategoryTrendCalculator : ICategoryTrendCalculator
    {
        public CategoryTrendMetrics Calculate(List<Expense> expenses, ExpenseCategory category, List<YearMonthsAnalyticsDto> monthlyTotals, decimal totalCategorySpent, decimal totalSpent)
        {
            var catExpenses = expenses.Where(e => e.Category == category).ToList();

            var highest = catExpenses.OrderByDescending(e => e.Amount).FirstOrDefault()?.Amount ?? 0;
            var lowest = catExpenses.OrderBy(e => e.Amount).FirstOrDefault()?.Amount ?? 0;

            var currMonth = monthlyTotals.Last().Total;
            var oldestMonth = monthlyTotals.First().Total;

            decimal percentageChange;

            if (oldestMonth == 0)
                percentageChange = currMonth == 0 ? 0 : 100;
            else
                percentageChange = Math.Round((currMonth - oldestMonth) / oldestMonth * 100, 2);

            var multiplier = Math.Round(1 + (percentageChange / 100m), 2);
            var avgMonthly = Math.Round(totalCategorySpent / monthlyTotals.Count, 2);

            return new CategoryTrendMetrics(
                CurrMonthAmount: currMonth,
                OldestMonthAmount: oldestMonth,
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
