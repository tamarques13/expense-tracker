namespace ExpenseTracker.Domain.Services.Interfaces
{
    public interface IDateRangeService
    {
        (DateOnly Start, DateOnly End) GetMonthRange(DateOnly date, int range);
        DateOnly GetPreviousMonth(DateOnly date);
        DateOnly Normalize(DateOnly date);
        IEnumerable<DateOnly> GetRollingMonths(DateOnly start, int range);
    }
}
