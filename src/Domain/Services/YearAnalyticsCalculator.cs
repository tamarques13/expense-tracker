using Spentir.Domain.Services.Interfaces;
using Spentir.Application.DTOs;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Domain.Services
{
    public class YearAnalyticsCalculator : IYearAnalyticsCalculator
    {
        public YearAnalyticsMetrics Calculate(List<Expense> expenses, decimal totalSpent, List<CategoryYearAnalyticsDto> categories, List<YearMonthsAnalyticsDto> monthly)
        {
            var highest = expenses.OrderByDescending(e => e.Amount).FirstOrDefault();
            var lowest = expenses.OrderBy(e => e.Amount).FirstOrDefault();
            var average = Math.Round(totalSpent / 12, 2);

            return new YearAnalyticsMetrics(
                Total: totalSpent,
                AverageMonthlySpent: average,
                HighestSpendingCategory: highest?.Category.ToString() ?? "N/A",
                HighestSpendingAmount: highest?.Amount ?? 0,
                LowestSpendingCategory: lowest?.Category.ToString() ?? "N/A",
                LowestSpendingAmount: lowest?.Amount ?? 0,
                Categories: categories,
                Months: monthly
            );
        }
    }
}