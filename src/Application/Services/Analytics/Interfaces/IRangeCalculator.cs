using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Application.Services.Analytics.Interfaces
{
    /// <summary>
    /// Provides based date range calculations for analytics.
    /// </summary>
    public interface IRangeCalculator
    {
        (DateOnly Start, DateOnly End) GetRange(DateOnly date, int months);
        (DateRange Current, DateRange Previous) GetMonthRanges(DateOnly date, int months);
        (DateOnly Start, DateOnly End) GetYearRange(DateOnly date);
    }
}
