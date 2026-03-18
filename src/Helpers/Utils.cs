using Spentir.Models;
using Spentir.DTOs;

namespace Spentir.Helpers
{
    public class Utils
    {
        /// <summary>
        /// Calculates the date range that represents the full calendar month
        /// of the provided date. The range starts on the first day of the month
        /// and ends on the first day of the following month.
        /// </summary>
        /// <param name="date">Any date within the month to be evaluated.</param>
        public static (DateOnly start, DateOnly end) GetMonthRange(DateOnly date)
        {
            var start = new DateOnly(date.Year, date.Month, 1);
            var end = start.AddMonths(1);

            return (start, end);
        }

        /// <summary>
        /// Calculates the rolling 12 month period that ends at the end of the month
        /// of the provided date. The range spans exactly 12 full months.
        /// </summary>
        /// <param name="date">Any date whose month will be considered the final month of the 12 month window.</param>
        /// 
        public static (DateOnly start, DateOnly end) GetLastYearRange(DateOnly date)
        {
            var end = new DateOnly(date.Year, date.Month, 1).AddMonths(1);
            var start = end.AddMonths(-12);

            return (start, end);
        }

        /// <summary>
        /// Calculates the rolling 6 month period that ends at the end of the month
        /// of the provided date. The range spans exactly 6 full months.
        /// </summary>
        /// <param name="date">Any date whose month will be considered the final month of the 6 month window.</param>
        /// 
        public static (DateOnly start, DateOnly end) GetLastXMonthRange(DateOnly date, int range)
        {
            var end = new DateOnly(date.Year, date.Month, 1).AddMonths(1);
            var start = end.AddMonths(-range);

            return (start, end);
        }

        /// <summary>
        /// Normalizes the provided date into a valid reporting month. If the input is null or uninitialized,
        /// the method defaults to the current system date to ensure consistent downstream processing.
        /// </summary>
        /// <param name="date">The optional date to normalize.</param>
        /// <returns>A valid <see cref="DateOnly"/> representing the target month.</returns>

        public static DateOnly NormalizeDate(DateOnly date)
        {
            return date == DateOnly.MinValue ? DateOnly.FromDateTime(DateTime.Now) : date;
        }

        /// <summary>
        /// Computes the first day of the month immediately preceding the specified date. This is used to
        /// align historical expense retrieval with monthly reporting boundaries.
        /// </summary>
        /// <param name="date">The reference date.</param>

        public static DateOnly GetPreviousMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1).AddMonths(-1);
        }

        /// <summary>
        /// Aggregates expenses by <see cref="ExpenseCategory"/> and computes total and average
        /// spending for each category. Produces a dictionary where each category present in the
        /// input sequence is mapped to its corresponding aggregated metrics.
        /// </summary>
        /// <param name="expenses">The collection of expenses to group and summarize.</param>

        public static Dictionary<ExpenseCategory, (decimal Total, decimal Average)?> GroupByTotalCategory(IEnumerable<Expense> expenses)
        {
            return expenses
                .GroupBy(e => e.Category)
                .ToDictionary(
                    g => g.Key,
                    g => ((decimal Total, decimal Average)?)(
                        g.Sum(x => x.Amount),
                        g.Average(x => x.Amount)
                    )
                );
        }

        /// <summary>
        /// Aggregates the provided expenses by category and computes total amount, transaction count
        /// and average transaction value for each category present in the dataset.
        /// </summary>
        /// <param name="expenses">The collection of expenses to group and aggregate.</param>

        public static Dictionary<ExpenseCategory, (decimal Total, int TransationsNum, decimal Average)?> GroupByCategory(IEnumerable<Expense> expenses)
        {
            return expenses
                .GroupBy(e => e.Category)
                .ToDictionary(
                    g => g.Key,
                    g => ((decimal Total, int TransationsNum, decimal Average)?)(
                        g.Sum(x => x.Amount),
                        g.Count(),
                        g.Average(x => x.Amount)
                    )
                );
        }

        /// <summary>
        /// Produces a simplified category aggregation containing only total spending per category.
        /// This is used for previous month comparisons where additional metrics such as averages
        /// or transaction counts are not required.
        /// </summary>
        /// <param name="expenses">The collection of expenses to aggregate.</param>

        public static Dictionary<ExpenseCategory, decimal> GroupByCategorySimple(IEnumerable<Expense> expenses)
        {
            return expenses.GroupBy(e => e.Category).ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));
        }

        /// <summary>
        /// Groups all expenses by their creation year and month, producing a dictionary
        /// where each key represents a specific (Year, Month) pair and the value is the
        /// total amount spent in that period.
        /// </summary>
        /// <param name="expenses">The list of expenses to be grouped.</param>

        public static Dictionary<(int Year, int Month), decimal> GroupExpensesByMonth(List<Expense> expenses)
        {
            return expenses.GroupBy(e => (e.CreatedAt.Year, e.CreatedAt.Month)).ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));
        }

        /// <summary>
        /// Generates a sequence of 12 consecutive months starting from the provided date.
        /// This is used to build rolling year analytics rather than a fixed calendar year.
        /// </summary>
        /// <param name="start">The first month of the 12‑month rolling period.</param>

        public static IEnumerable<DateOnly> GetRollingMonths(DateOnly start, int range)
        {
            var current = start;

            for (int i = 0; i < range; i++)
            {
                yield return current;
                current = current.AddMonths(1);
            }
        }


        /// <summary>
        /// Constructs a complete set of category analytics DTOs. Ensures that all categories defined
        /// in <see cref="ExpenseCategory"/> are represented, even if no expenses exist for a category
        /// in the current month. Computes totals, averages, transaction counts and percentage-of-total
        /// metrics for each category.
        /// </summary>
        /// <param name="grouped">The aggregated category metrics for the current month.</param>
        /// <param name="totalSpent">The total amount spent in the current month.</param>

        public static List<CategoryAnalyticsDto> BuildCategoryDtos(Dictionary<ExpenseCategory, (decimal Total, int TransationsNum, decimal Average)?> grouped, decimal totalSpent)
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
        /// Enhances each category analytics DTO with previous month totals and computes the absolute
        /// month-over-month change. This enables comparative insights across reporting periods.
        /// </summary>
        /// <param name="categories">The category DTOs to enrich with comparison data.</param>
        /// <param name="previousExpenses">The expenses from the previous month.</param>

        public static void ApplyPreviousMonthComparison(List<CategoryAnalyticsDto> categories, IEnumerable<Expense> previousExpenses)
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
        /// Builds a complete set of annual category analytics DTOs. Ensures that all categories
        /// defined in <see cref="ExpenseCategory"/> are included, even if no expenses were recorded
        /// for a given category during the year. Calculates total spending and each category’s
        /// percentage contribution to the yearly total.
        /// </summary>
        /// <param name="grouped">The aggregated yearly metrics per category, containing total and average values.</param>
        /// <param name="totalSpent">The total amount spent across all categories during the year.</param>

        public static List<CategoryYearAnalyticsDto> BuildYearCategoryDtos(Dictionary<ExpenseCategory, (decimal Total, decimal Average)?> grouped, decimal totalSpent)
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
        /// Computes high-level spending trends between the current and previous month, including
        /// total previous-month spending, absolute change and percentage change. These metrics
        /// support summary trend visualizations and financial insights.
        /// </summary>
        /// <param name="current">The current month's expenses.</param>
        /// <param name="previous">The previous month's expenses.</param>

        public static TrendAnalyticsDto BuildTrendAnalytics(IEnumerable<Expense> current, IEnumerable<Expense> previous)
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

        public static List<MonthlyAnalyticsDto> BuildMontlyAnalytics(List<Expense> expenses, decimal totalSpent, DateOnly start, int range)
        {
            var result = new List<MonthlyAnalyticsDto>(range);
            var grouped = GroupExpensesByMonth(expenses);
            var months = GetRollingMonths(start, range);

            foreach (var date in months)
            {
                grouped.TryGetValue((date.Year, date.Month), out var monthTotal);

                var dto = new MonthlyAnalyticsDto
                {
                    Year = date.Year,
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

        public static MonthAnalyticsDto BuildMonthSummary(DateOnly date, List<Expense> expenses, decimal totalSpent, List<CategoryAnalyticsDto> categories, TrendAnalyticsDto trend)
        {
            var sortedAmounts = expenses.Select(e => e.Amount).OrderBy(x => x).ToList();
            var median = sortedAmounts.Count == 0 ? 0 : sortedAmounts[sortedAmounts.Count / 2];

            var highest = expenses.OrderByDescending(e => e.Amount).FirstOrDefault();

            return new MonthAnalyticsDto
            {
                Year = date.Year,
                Month = date.Month,
                Total = totalSpent,
                AverageDailySpent = Math.Round(totalSpent / DateTime.DaysInMonth(date.Year, date.Month), 2),
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


        public static YearAnalyticsDto BuildYearMonthSummary(DateOnly date, List<Expense> expenses, decimal totalSpent, List<CategoryYearAnalyticsDto> categories, List<MonthlyAnalyticsDto> monthly)
        {
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

        public static CategoryTrendDto BuildCategorySummary(List<Expense> expenses, decimal totalCatSpent, decimal totalSpent, ExpenseCategory category, List<MonthlyAnalyticsDto> monthlyTotals)
        {
            decimal percentageChange;
            var highest = expenses.OrderByDescending(e => e.Amount).FirstOrDefault(e => e.Category == category);
            var lowest = expenses.OrderBy(e => e.Amount).FirstOrDefault(e => e.Category == category);

            var currMonth = monthlyTotals.Last().Total;
            var oldestMonth = monthlyTotals.First().Total;

            if (oldestMonth == 0)
                percentageChange = currMonth == 0 ? 0 : 100;
            else
                percentageChange = Math.Round((currMonth - oldestMonth) / oldestMonth * 100, 2);

            return new CategoryTrendDto
            {
                CategoryName = category.ToString(),
                Category = category,
                CurrMonthAmount = currMonth,
                AverageMonthlySpent = Math.Round(totalCatSpent / 6, 2),
                TotalSpent = totalCatSpent,
                HighestAmount = highest?.Amount ?? 0,
                LowestAmount = lowest?.Amount ?? 0,
                PercentageOfTotalYear = Math.Round(totalCatSpent / totalSpent * 100, 2),
                PercentageChange = percentageChange,
                Multiplier = Math.Round(1 + (percentageChange / 100m), 2),
                IsImproving = percentageChange < 0
            };
        }
    }
}