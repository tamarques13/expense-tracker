using ExpenseTracker.Domain.Services.Interfaces;

namespace ExpenseTracker.Domain.Services
{
    /// <summary>
    /// Domain service that provides date normalization and range calculations used
    /// throughout the analytics and expense domains. Supplies month ranges,
    /// previous‑month resolution and rolling month sequences for multi‑period analysis.
    /// </summary>

    public class DateRangeService : IDateRangeService
    {
        public (DateOnly Start, DateOnly End) GetMonthRange(DateOnly date, int range)
        {
            var end = new DateOnly(date.Year, date.Month, 1).AddMonths(1);
            var start = end.AddMonths(-range);

            return (start, end);
        }

        public DateOnly GetPreviousMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1).AddMonths(-1);
        }

        public DateOnly Normalize(DateOnly date)
        {
            return date == DateOnly.MinValue ? DateOnly.FromDateTime(DateTime.Now) : date;
        }

        /// <summary>
        /// Generates a sequence of 12 consecutive months starting from the provided date.
        /// This is used to build rolling year analytics rather than a fixed calendar year.
        /// </summary>
        /// <param name="start">The first month of the 12‑month rolling period.</param>

        public IEnumerable<DateOnly> GetRollingMonths(DateOnly start, int range)
        {
            var current = start;

            for (int i = 0; i < range; i++)
            {
                yield return current;
                current = current.AddMonths(1);
            }
        }
    }
}