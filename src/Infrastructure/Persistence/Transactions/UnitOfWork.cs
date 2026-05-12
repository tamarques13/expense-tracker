using ExpenseTracker.Infrastructure.Persistence.Transactions.Interfaces;
using ExpenseTracker.Infrastructure.Persistence.Configurations;

namespace ExpenseTracker.Infrastructure.Persistence.Transactions
{
    /// <summary>
    /// Provides a transactional execution boundary for operations that must succeed or fail as a unit.
    /// 
    /// This implementation:
    /// - Wraps EF Core database transactions
    /// - Ensures atomic execution of multi-step operations
    /// - Rolls back changes on failure to maintain data consistency
    /// 
    /// The unit of work does not contain business logic; it simply guarantees
    /// that the provided action executes within a reliable transactional scope.
    /// </summary>

    public class UnitOfWork(SpentirDbContext context) : IUnitOfWork
    {
        private readonly SpentirDbContext _context = context;

        /// <summary>
        /// Executes the provided asynchronous action within a database transaction.
        /// If the action completes successfully, the transaction is committed.
        /// If an exception occurs, the transaction is rolled back.
        /// </summary>
        /// <param name="action">
        /// The operation to execute transactionally.  
        /// All database changes performed inside this delegate are committed or rolled back together.
        /// </param>
        /// <param name="cancellationToken">
        /// Token used to cancel the transaction or the execution of the action.
        /// </param>

        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await action();
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
