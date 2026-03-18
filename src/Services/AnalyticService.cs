using Spentir.Repositories.Interfaces;
using Spentir.Services.Interfaces;
using Spentir.Models;
using Spentir.DTOs;
using Spentir.Helpers;

namespace Spentir.Services
{
    public class AnalyticService(IExpenseRepository expenseRepository) : IAnalyticService
    {
        private readonly IExpenseRepository _expenseRepository = expenseRepository;

        /// <summary>
        /// Generates the complete analytics report for a given month and user. This method orchestrates
        /// the full analytics pipeline, including data retrieval, category aggregation, previous-month
        /// comparison, trend calculation and summary construction.
        /// </summary>
        /// <param name="date">The target month to analyze. If null or invalid, the current month is used.</param>
        /// <param name="userId">The identifier of the user whose expenses are being analyzed.</param>

        public async Task<MonthAnalyticsDto> GetMonthAnalyticsAsync(DateOnly? date, Guid userId)
        {
            if (!date.HasValue) throw new DomainException("Date is required.");
            var targetDate = Utils.NormalizeDate(date.Value);

            (DateOnly currStart, DateOnly currEnd) = Utils.GetMonthRange(targetDate);
            (DateOnly prevStart, DateOnly prevEnd) = Utils.GetMonthRange(Utils.GetPreviousMonth(targetDate));

            var currentExpenses = await LoadExpenses(userId, currStart, currEnd);
            var previousExpenses = await LoadExpenses(userId, prevStart, prevEnd);

            var totalSpent = currentExpenses.Sum(e => e.Amount);

            var grouped = Utils.GroupByCategory(currentExpenses);
            var categories = Utils.BuildCategoryDtos(grouped, totalSpent);

            Utils.ApplyPreviousMonthComparison(categories, previousExpenses);

            var trend = Utils.BuildTrendAnalytics(currentExpenses, previousExpenses);
            var summary = Utils.BuildMonthSummary(targetDate, currentExpenses, totalSpent, categories, trend);

            return summary;
        }

        /// <summary>
        /// Retrieves and builds the full analytics summary for the previous year relative to the
        /// provided date. Validates the input date, loads all expenses for the computed yearly
        /// range, aggregates totals and category-level metrics, and prepares the final yearly
        /// analytics DTO.
        /// </summary>
        /// <param name="date">The reference date used to determine the target year range. Must not be null.</param>
        /// <param name="userId">The identifier of the user whose expenses will be analyzed.</param>
        /// <exception cref="DomainException">Thrown when the provided date is null.</exception>

        public async Task<YearAnalyticsDto> GetYearAnalyticsAsync(DateOnly? date, Guid userId)
        {
            if (!date.HasValue) throw new DomainException("Date is required.");
            var targetDate = Utils.NormalizeDate(date.Value);

            (DateOnly start, DateOnly end) = Utils.GetLastYearRange(targetDate);
            var yearExpenses = (await _expenseRepository.GetAsync(userId, start, end)).ToList();

            var totalSpent = yearExpenses.Sum(e => e.Amount);

            var grouped = Utils.GroupByTotalCategory(yearExpenses);
            var categories = Utils.BuildYearCategoryDtos(grouped, totalSpent);
            var monthly = Utils.BuildMontlyAnalytics(yearExpenses, totalSpent, start, 12);
            var summary = Utils.BuildYearMonthSummary(targetDate, yearExpenses, totalSpent, categories, monthly);

            return summary;
        }

        public async Task<CategoryTrendDto> GetCategoryTrendAsync(ExpenseCategory category, DateOnly? date, int range, Guid userId)
        {
            if (!date.HasValue) throw new DomainException("Date is required.");
            if (range < 2) throw new DomainException("Range must be greater than 1");

            var targetDate = Utils.NormalizeDate(date.Value);

            (DateOnly start, DateOnly End) = Utils.GetLastXMonthRange(targetDate, range);

            var lastMonthsExpenses = await LoadExpenses(userId, start, End);

            var totalLastMonthsSpent = lastMonthsExpenses.Sum(e => e.Amount);
            var totalCatSpent = lastMonthsExpenses.Where(e => e.Category == category).Sum(e => e.Amount);

            var lastMonthsCatExpenses = lastMonthsExpenses.Where(e => e.Category == category).ToList();

            var monthlyTotals = Utils.BuildMontlyAnalytics(lastMonthsCatExpenses, totalLastMonthsSpent, start, range);
            var summary = Utils.BuildCategorySummary(lastMonthsExpenses, totalCatSpent, totalLastMonthsSpent, category, monthlyTotals);

            return summary;
        }

        /// <summary>
        /// Retrieves all expenses for the specified user and month from the repository. The results are
        /// materialized into a list to support multiple enumerations and sorting operations during analytics.
        /// </summary>
        /// <param name="userId">The identifier of the user whose expenses are being loaded.</param>
        /// <param name="date">The month for which expenses should be retrieved.</param>

        private async Task<List<Expense>> LoadExpenses(Guid userId, DateOnly start, DateOnly end)
        {
            return (await _expenseRepository.GetAsync(userId, start, end)).ToList();
        }
    }
}