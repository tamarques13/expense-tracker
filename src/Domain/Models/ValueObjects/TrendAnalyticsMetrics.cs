namespace Spentir.Domain.Models.ValueObjects
{
    public record TrendAnalyticsMetrics(
        decimal PreviousTotal,
        decimal Change,
        decimal MonthChange
    );
}
