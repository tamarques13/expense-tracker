namespace Spentir.Domain.Models
{
    public record CategoryAggregate(
        decimal Total,
        int Transactions,
        decimal Average
    );
}
