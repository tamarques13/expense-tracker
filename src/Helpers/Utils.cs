using Spentir.Models;

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
        /// <param name="date">Any date whose month will be considered the final month of the 12‑month window.</param>
        /// 
        public static (DateOnly start, DateOnly end) GetLastYearRange(DateOnly date)
        {
            var end = new DateOnly(date.Year, date.Month, 1).AddMonths(1);
            var start = end.AddMonths(-12);

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

        public static IEnumerable<DateOnly> GetRollingMonths(DateOnly start)
        {
            var current = start;

            for (int i = 0; i < 12; i++)
            {
                yield return current;
                current = current.AddMonths(1);
            }
        }
    }
}