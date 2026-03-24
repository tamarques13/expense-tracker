namespace Spentir.Infrastructure.Persistence.Transactions.Interfaces
{
    public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
    }
}
