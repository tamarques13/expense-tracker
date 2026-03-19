using Spentir.Repositories.Interfaces;
using Spentir.Services.Interfaces;
using Spentir.Domain.Services.Interfaces;
using Spentir.Models;
using Spentir.DTOs;
using Spentir.Helpers;
using Spentir.Helpers.Builders;


namespace Spentir.Services
{
    public class AnalyticService(
        IExpenseRepository expenseRepository,
        IDateRangeService dateRangeService,
        ICategoryAggregateService aggregateService,
        ICategoryTrendCalculator categoryTrendCalculator,
        IMonthAnalyticsCalculator monthAnalyticsCalculator,
        ITrendAnalyticsCalculator trendAnalyticsCalculator,
        IYearAnalyticsCalculator yearAnalyticsCalculator
        ) : IAnalyticService
    {
        private readonly IExpenseRepository _expenseRepository = expenseRepository;
        private readonly IDateRangeService _dateRangeService = dateRangeService;
        private readonly ICategoryAggregateService _aggregateService = aggregateService;
        private readonly ICategoryTrendCalculator _categoryTrendCalculator = categoryTrendCalculator;
        private readonly IMonthAnalyticsCalculator _monthAnalyticsCalculator = monthAnalyticsCalculator;
        private readonly ITrendAnalyticsCalculator _trendAnalyticsCalculator = trendAnalyticsCalculator;
        private readonly IYearAnalyticsCalculator _yearAnalyticsCalculator = yearAnalyticsCalculator;

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
            var targetDate = _dateRangeService.Normalize(date.Value);

            (DateOnly currStart, DateOnly currEnd) = _dateRangeService.GetMonthRange(targetDate, 1);
            (DateOnly prevStart, DateOnly prevEnd) = _dateRangeService.GetMonthRange(_dateRangeService.GetPreviousMonth(targetDate), 1);

            var currentExpenses = (await _expenseRepository.GetAsync(userId, currStart, currEnd)).ToList();
            var previousExpenses = (await _expenseRepository.GetAsync(userId, prevStart, prevEnd)).ToList();

            var totalSpent = currentExpenses.Sum(e => e.Amount);
            var grouped = _aggregateService.GroupByCategory(currentExpenses);
            var groupedPrev = _aggregateService.GroupByCategorySimple(previousExpenses);

            var categories = CategoryAnalyticsDtoBuilder.Build(grouped, totalSpent);
            CategoryAnalyticsDtoBuilder.ApplyPreviousMonthComparison(categories, groupedPrev);

            var trendMetrics = _trendAnalyticsCalculator.Calculate(currentExpenses, previousExpenses);
            var trend = TrendAnalyticsDtoBuilder.Build(trendMetrics);

            var metrics = _monthAnalyticsCalculator.Calculate(targetDate, currentExpenses, totalSpent, categories, trend);

            return MonthAnalyticsDtoBuilder.Build(metrics, targetDate);
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
            var targetDate = _dateRangeService.Normalize(date.Value);

            (DateOnly start, DateOnly end) = _dateRangeService.GetMonthRange(targetDate, 12);
            var yearExpenses = (await _expenseRepository.GetAsync(userId, start, end)).ToList();

            var totalSpent = yearExpenses.Sum(e => e.Amount);

            var grouped = _aggregateService.GroupByCategory(yearExpenses);
            var categories = CategoryYearAnalyticsDtoBuilder.Build(grouped, totalSpent);

            var monthGrouped = _aggregateService.GroupExpensesByMonth(yearExpenses);
            var months = _dateRangeService.GetRollingMonths(start, 12);

            var monthly = MonthlyAnalyticsDtoBuilder.Build(totalSpent, monthGrouped, months);
            var metrics = _yearAnalyticsCalculator.Calculate(yearExpenses, totalSpent, categories, monthly);

            return YearAnalyticsDtoBuilder.Build(targetDate, metrics);
        }

        /// <summary>
        /// Computes the spending trend for a specific category over a rolling range of months.
        /// Validates the input parameters, determines the target date window, loads all expenses
        /// for the selected period, aggregates category-level totals, builds month-by-month
        /// analytics.
        /// </summary>
        /// <param name="category">The expense category for which the trend will be calculated.</param>
        /// <param name="date">The reference date used to determine the rolling month range.</param>
        /// <param name="range">The number of months to include in the trend calculation. Must be greater than 1.</param>
        /// <param name="userId">The identifier of the user whose expenses will be analyzed.</param>
        /// <exception cref="DomainException">Thrown when the provided date is null or when the range is less than 2.</exception>

        public async Task<CategoryTrendDto> GetCategoryTrendAsync(ExpenseCategory category, DateOnly? date, int range, Guid userId)
        {
            if (!date.HasValue) throw new DomainException("Date is required.");
            if (range < 2) throw new DomainException("Range must be greater than 1");

            var targetDate = _dateRangeService.Normalize(date.Value);

            (DateOnly start, DateOnly end) = _dateRangeService.GetMonthRange(targetDate, range);

            var lastMonthsExpenses = (await _expenseRepository.GetAsync(userId, start, end)).ToList();

            var totalLastMonthsSpent = lastMonthsExpenses.Sum(e => e.Amount);
            var totalCatSpent = lastMonthsExpenses.Where(e => e.Category == category).Sum(e => e.Amount);

            var lastMonthsCatExpenses = lastMonthsExpenses.Where(e => e.Category == category).ToList();

            var monthGrouped = _aggregateService.GroupExpensesByMonth(lastMonthsCatExpenses);
            var months = _dateRangeService.GetRollingMonths(start, range);

            var monthlyTotals = MonthlyAnalyticsDtoBuilder.Build(totalLastMonthsSpent, monthGrouped, months);

            var metrics = _categoryTrendCalculator.Calculate(lastMonthsExpenses, category, monthlyTotals, totalCatSpent, totalLastMonthsSpent);

            return CategoryTrendDtoBuilder.Build(metrics, category);
        }
    }
}