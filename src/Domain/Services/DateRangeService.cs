using Spentir.Domain.Services.Interfaces;

namespace Spentir.Domain.Services
{
    public class DateRangeService : IDateRangeService
    {
        /// <summary>
        /// Calculates the rolling x month period that ends at the end of the month
        /// of the provided date. The range spans exactly x full months.
        /// </summary>
        /// <param name="date">Any date whose month will be considered the final month of the x month window.</param>

        public (DateOnly Start, DateOnly End) GetMonthRange(DateOnly date, int range)
        {
            var end = new DateOnly(date.Year, date.Month, 1).AddMonths(1);
            var start = end.AddMonths(-range);

            return (start, end);
        }

        /// <summary>
        /// Computes the first day of the month immediately preceding the specified date. This is used to
        /// align historical expense retrieval with monthly reporting boundaries.
        /// </summary>
        /// <param name="date">The reference date.</param>

        public DateOnly GetPreviousMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1).AddMonths(-1);
        }

        /// <summary>
        /// Normalizes the provided date into a valid reporting month. If the input is null or uninitialized,
        /// the method defaults to the current system date to ensure consistent downstream processing.
        /// </summary>
        /// <param name="date">The optional date to normalize.</param>
        /// <returns>A valid <see cref="DateOnly"/> representing the target month.</returns>

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