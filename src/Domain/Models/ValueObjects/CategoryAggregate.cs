namespace Spentir.Domain.Models.ValueObjects
{
    public record CategoryAggregate(
        decimal Total,
        int Transactions,
        decimal Average
    );
}
