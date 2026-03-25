using Spentir.Application.Services.Analytics.Interfaces;
using Spentir.Domain.Models.ValueObjects;
using Spentir.Domain.Services.Interfaces;

namespace Spentir.Application.Services.Analytics.Ranges
{
    /// <summary>
    /// Calculates date ranges for analytics.
    /// </summary>

    public class RangeCalculator(IDateRangeService dateRangeService) : IRangeCalculator
    {
        private readonly IDateRangeService _dateRangeService = dateRangeService;

        public (DateOnly Start, DateOnly End) GetRange(DateOnly date, int months)
        {
            return _dateRangeService.GetMonthRange(date, months);
        }

        public (DateRange Current, DateRange Previous) GetMonthRanges(DateOnly date, int months)
        {
            var (currStart, currEnd) = _dateRangeService.GetMonthRange(date, months);
            var (prevStart, prevEnd) = _dateRangeService.GetMonthRange(_dateRangeService.GetPreviousMonth(date), months);

            return (new DateRange(currStart, currEnd), new DateRange(prevStart, prevEnd));
        }

        public (DateOnly Start, DateOnly End) GetYearRange(DateOnly date)
        {
            return _dateRangeService.GetMonthRange(date, 12);
        }
    }
}
