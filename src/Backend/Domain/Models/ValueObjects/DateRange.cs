namespace ExpenseTracker.Domain.Models.ValueObjects
{
    public readonly record struct DateRange(DateOnly Start, DateOnly End);
}

