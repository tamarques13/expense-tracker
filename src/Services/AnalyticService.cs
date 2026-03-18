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
            var categories = BuildCategoryDtos(grouped, totalSpent);

            ApplyPreviousMonthComparison(categories, previousExpenses);

            var trend = BuildTrendAnalytics(currentExpenses, previousExpenses);
            var summary = BuildMonthSummary(targetDate, currentExpenses, totalSpent, categories, trend);

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
            var yearExpenses = await LoadExpenses(userId, start, end);

            var totalSpent = yearExpenses.Sum(e => e.Amount);

            var grouped = Utils.GroupByTotalCategory(yearExpenses);
            var categories = BuildYearCategoryDtos(grouped, totalSpent);
            var monthly = BuildMontlyAnalytics(yearExpenses, totalSpent, start);
            var summary = BuildYearMonthSummary(targetDate, yearExpenses, totalSpent, categories, monthly);

            return summary;
        }

        public Task<CategoryTrendDto> GetCategoryTrendAsync(ExpenseCategory category, DateOnly? date, Guid userId)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Constructs a complete set of category analytics DTOs. Ensures that all categories defined
        /// in <see cref="ExpenseCategory"/> are represented, even if no expenses exist for a category
        /// in the current month. Computes totals, averages, transaction counts and percentage-of-total
        /// metrics for each category.
        /// </summary>
        /// <param name="grouped">The aggregated category metrics for the current month.</param>
        /// <param name="totalSpent">The total amount spent in the current month.</param>

        private static List<CategoryAnalyticsDto> BuildCategoryDtos(Dictionary<ExpenseCategory, (decimal Total, int TransationsNum, decimal Average)?> grouped, decimal totalSpent)
        {
            return Enum.GetValues<ExpenseCategory>().Select(c =>
                {
                    grouped.TryGetValue(c, out var data);

                    var total = data?.Total ?? 0;

                    return new CategoryAnalyticsDto
                    {
                        CategoryName = c.ToString(),
                        Category = c,
                        Total = total,
                        TransationsNum = data?.TransationsNum ?? 0,
                        Average = data?.Average ?? 0,
                        Percentage = totalSpent == 0 ? 0 : Math.Round(total / totalSpent * 100, 2)
                    };
                })
                .ToList();
        }

        /// <summary>
        /// Builds a complete set of annual category analytics DTOs. Ensures that all categories
        /// defined in <see cref="ExpenseCategory"/> are included, even if no expenses were recorded
        /// for a given category during the year. Calculates total spending and each category’s
        /// percentage contribution to the yearly total.
        /// </summary>
        /// <param name="grouped">The aggregated yearly metrics per category, containing total and average values.</param>
        /// <param name="totalSpent">The total amount spent across all categories during the year.</param>


        private static List<CategoryYearAnalyticsDto> BuildYearCategoryDtos(Dictionary<ExpenseCategory, (decimal Total, decimal Average)?> grouped, decimal totalSpent)
        {
            return Enum.GetValues<ExpenseCategory>().Select(c =>
                {
                    grouped.TryGetValue(c, out var data);

                    var total = data?.Total ?? 0;

                    return new CategoryYearAnalyticsDto
                    {
                        CategoryName = c.ToString(),
                        Category = c,
                        Total = total,
                        Percentage = totalSpent == 0 ? 0 : Math.Round(total / totalSpent * 100, 2)
                    };
                })
                .ToList();
        }

        /// <summary>
        /// Enhances each category analytics DTO with previous month totals and computes the absolute
        /// month-over-month change. This enables comparative insights across reporting periods.
        /// </summary>
        /// <param name="categories">The category DTOs to enrich with comparison data.</param>
        /// <param name="previousExpenses">The expenses from the previous month.</param>

        private static void ApplyPreviousMonthComparison(List<CategoryAnalyticsDto> categories, IEnumerable<Expense> previousExpenses)
        {
            var groupedPrev = Utils.GroupByCategorySimple(previousExpenses);

            foreach (var c in categories)
            {
                groupedPrev.TryGetValue(c.Category, out var prevTotal);
                c.PreviousMonthTotal = prevTotal;
                c.LastMonthChange = c.Total - prevTotal;
            }
        }

        /// <summary>
        /// Computes high-level spending trends between the current and previous month, including
        /// total previous-month spending, absolute change and percentage change. These metrics
        /// support summary trend visualizations and financial insights.
        /// </summary>
        /// <param name="current">The current month's expenses.</param>
        /// <param name="previous">The previous month's expenses.</param>

        private static TrendAnalyticsDto BuildTrendAnalytics(IEnumerable<Expense> current, IEnumerable<Expense> previous)
        {
            var currentTotal = current.Sum(e => e.Amount);
            var previousTotal = previous.Sum(e => e.Amount);

            var change = currentTotal - previousTotal;

            return new TrendAnalyticsDto
            {
                PreviousMonthTotal = previousTotal,
                LastMonthChange = change,
                MonthPercentageChange = previousTotal == 0 ? 0 : Math.Round(change / previousTotal * 100, 2)
            };
        }

        /// <summary>
        /// Produces a month by month spending summary for a full year by grouping all expenses
        /// according to their creation month and aggregating the total amount spent in each period.
        /// </summary>
        /// <param name="expenses">The list of expenses for the current month.</param>

        private static List<MonthlyYearAnalyticsDto> BuildMontlyAnalytics(List<Expense> expenses, decimal totalSpent, DateOnly start)
        {
            var result = new List<MonthlyYearAnalyticsDto>(12);
            var grouped = Utils.GroupExpensesByMonth(expenses);
            var months = Utils.GetRollingMonths(start);

            foreach (var date in months)
            {
                grouped.TryGetValue((date.Year, date.Month), out var monthTotal);

                var dto = new MonthlyYearAnalyticsDto
                {
                    Month = date.Month,
                    Total = monthTotal,
                    Percentage = totalSpent == 0 ? 0 : Math.Round(monthTotal / totalSpent * 100, 2)
                };

                result.Add(dto);
            }

            return result;
        }

        /// <summary>
        /// Constructs the final monthly analytics summary, including global statistics such as total
        /// spending, average daily spending, median expense and largest expense. Attaches category-level
        /// analytics and trend metrics to produce a complete <see cref="MonthAnalyticsDto"/>.
        /// </summary>
        /// <param name="date">The reporting month.</param>
        /// <param name="expenses">The list of expenses for the current month.</param>
        /// <param name="totalSpent">The total amount spent in the current month.</param>
        /// <param name="categories">The computed category analytics.</param>
        /// <param name="trend">The month-over-month trend metrics.</param>

        private static MonthAnalyticsDto BuildMonthSummary(DateOnly date, List<Expense> expenses, decimal totalSpent, List<CategoryAnalyticsDto> categories, TrendAnalyticsDto trend)
        {
            var sortedAmounts = expenses.Select(e => e.Amount).OrderBy(x => x).ToList();
            var median = sortedAmounts.Count == 0 ? 0 : sortedAmounts[sortedAmounts.Count / 2];

            var highest = expenses.OrderByDescending(e => e.Amount).FirstOrDefault();

            return new MonthAnalyticsDto
            {
                Year = date.Year,
                Month = date.Month,
                Total = totalSpent,
                AverageDailySpent = totalSpent / DateTime.DaysInMonth(date.Year, date.Month),
                MedianExpense = median,
                LargestExpense = highest?.Amount ?? 0,
                LargestExpenseCategoryName = highest?.Category.ToString() ?? "N/A",
                LargestExpenseCategory = highest?.Category,
                Categories = categories,
                Trend = trend
            };
        }

        /// <summary>
        /// Constructs the final yearly analytics summary, including global statistics such as total
        /// spending, average category spending, highest and lowest spending category. .
        /// </summary>
        /// <param name="date">The month and year for which the summary is being generated.</param>
        /// <param name="expenses">The list of expenses recorded during the specified month.</param>
        /// <param name="totalSpent">The total amount spent during the month.</param>
        /// <param name="categories">The precomputed category-level analytics for the month.</param>


        private static YearAnalyticsDto BuildYearMonthSummary(DateOnly date, List<Expense> expenses, decimal totalSpent, List<CategoryYearAnalyticsDto> categories, List<MonthlyYearAnalyticsDto> monthly)
        {
            var sortedAmounts = expenses.Select(e => e.Amount).OrderBy(x => x).ToList();
            var median = sortedAmounts.Count == 0 ? 0 : sortedAmounts[sortedAmounts.Count / 2];

            var highest = expenses.OrderByDescending(e => e.Amount).FirstOrDefault();
            var lowest = expenses.OrderBy(e => e.Amount).FirstOrDefault();

            return new YearAnalyticsDto
            {
                Year = date.Year,
                Total = totalSpent,
                AverageMonthlySpent = Math.Round(totalSpent / 12, 2),
                HighestSpendingCategory = highest?.Category.ToString() ?? "N/A",
                HighestSpendingAmount = highest?.Amount ?? 0,
                LowestSpendingCategory = lowest?.Category.ToString() ?? "N/A",
                LowestSpendingAmount = lowest?.Amount ?? 0,
                Categories = categories,
                Months = monthly
            };
        }
    }

}