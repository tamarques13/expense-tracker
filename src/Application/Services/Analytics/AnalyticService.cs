using Spentir.Application.Services.Analytics.Interfaces;
using Spentir.Application.Mappers.Analytics;
using Spentir.Application.DTOs.Analytics;
using Spentir.Domain.Services.Interfaces;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Exceptions;

namespace Spentir.Application.Services.Analytics
{
    /// <summary>
    /// Application layer service responsible for generating all analytics reports.
    /// Orchestrates the full analytics pipeline by coordinating date range calculation,
    /// expense loading, category aggregation, trend computation and DTO construction.
    /// This service acts as the main entry point for monthly, yearly and category trend
    /// analytics for a given user.
    /// </summary>
    
    public class AnalyticService(
        IDateRangeService dateRangeService,
        ICategoryAggregateService aggregateService,
        ICategoryTrendCalculator categoryTrendCalculator,
        IMonthAnalyticsCalculator monthAnalyticsCalculator,
        IYearAnalyticsCalculator yearAnalyticsCalculator,
        IAnalyticsBuilder analyticsBuilder,
        IExpenseLoader expenseLoader,
        IRangeCalculator rangeCalculator
        ) : IAnalyticService
    {
        private readonly record struct DateRange(DateOnly Start, DateOnly End);
        private readonly IDateRangeService _dateRangeService = dateRangeService;
        private readonly ICategoryAggregateService _aggregateService = aggregateService;
        private readonly ICategoryTrendCalculator _categoryTrendCalculator = categoryTrendCalculator;
        private readonly IMonthAnalyticsCalculator _monthAnalyticsCalculator = monthAnalyticsCalculator;
        private readonly IYearAnalyticsCalculator _yearAnalyticsCalculator = yearAnalyticsCalculator;
        private readonly IAnalyticsBuilder _analyticsBuilder = analyticsBuilder;
        private readonly IExpenseLoader _expenseLoader = expenseLoader;
        private readonly IRangeCalculator _rangeCalculator = rangeCalculator;

        /// <summary>
        /// Generates the complete analytics report for a given month and user. This method orchestrates
        /// the full analytics pipeline, including data retrieval, category aggregation, previous-month
        /// comparison, trend calculation and summary construction.
        /// </summary>
        /// <param name="date">The target month to analyze. If null or invalid, the current month is used.</param>
        /// <param name="userId">The identifier of the user whose expenses are being analyzed.</param>

        public async Task<MonthAnalyticsDto> GetMonthAnalyticsAsync(DateOnly? date, Guid userId)
        {
            if (!date.HasValue)
                throw new DomainException("Date is required.");

            var targetDate = _dateRangeService.Normalize(date.Value);
            var (currentRange, previousRange) = _rangeCalculator.GetMonthRanges(targetDate, 1);

            var currentExpenses = await _expenseLoader.LoadAsync(userId, currentRange.Start, currentRange.End);
            var previousExpenses = await _expenseLoader.LoadAsync(userId, previousRange.Start, previousRange.End);

            var totalSpent = currentExpenses.Sum(e => e.Amount);

            var currentCategoryGroups = _aggregateService.GroupByCategory(currentExpenses);
            var previousCategoryGroups = _aggregateService.GroupByCategorySimple(previousExpenses);

            var categories = _analyticsBuilder.BuildCategoryAnalytics(currentCategoryGroups, previousCategoryGroups, totalSpent);

            var metrics = _monthAnalyticsCalculator.Calculate(
                targetDate,
                currentExpenses,
                currentCategoryGroups,
                totalSpent, categories,
                previousExpenses);

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
            if (!date.HasValue)
                throw new DomainException("Date is required.");

            var targetDate = _dateRangeService.Normalize(date.Value);
            (DateOnly Start, DateOnly End) = _rangeCalculator.GetRange(targetDate, 12);

            var yearExpenses = await _expenseLoader.LoadAsync(userId, Start, End);

            var totalSpent = yearExpenses.Sum(e => e.Amount);

            var categoryGroups = _aggregateService.GroupByCategory(yearExpenses);
            var expensesGroups = _aggregateService.GroupExpensesByMonth(yearExpenses);

            var categories = CategoryYearAnalyticsDtoBuilder.Build(categoryGroups, totalSpent);
            var monthlyAnalytics = _analyticsBuilder.BuildMonthlyAnalytics((Start, End), 12, totalSpent, expensesGroups);

            var metrics = _yearAnalyticsCalculator.Calculate(
                categoryGroups,
                totalSpent,
                categories,
                monthlyAnalytics);

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
            if (!date.HasValue)
                throw new DomainException("Date is required.");
            if (range < 2)
                throw new DomainException("Range must be greater than 1");

            var targetDate = _dateRangeService.Normalize(date.Value);
            (DateOnly Start, DateOnly End) = _rangeCalculator.GetRange(targetDate, range);

            var monthExpenses = await _expenseLoader.LoadAsync(userId, Start, End);
            var categoryExpenses = monthExpenses.Where(e => e.Category == category).ToList();

            var total = monthExpenses.Sum(e => e.Amount);
            var categoryTotal = monthExpenses.Where(e => e.Category == category).Sum(e => e.Amount);

            var expensesGroups = _aggregateService.GroupExpensesByMonth(categoryExpenses);

            var monthlyAnalytics = _analyticsBuilder.BuildMonthlyAnalytics((Start, End), range, total, expensesGroups);

            var metrics = _categoryTrendCalculator.Calculate(
                monthExpenses,
                category,
                monthlyAnalytics,
                categoryTotal,
                total);

            return CategoryTrendDtoBuilder.Build(metrics, category);
        }
    }
}