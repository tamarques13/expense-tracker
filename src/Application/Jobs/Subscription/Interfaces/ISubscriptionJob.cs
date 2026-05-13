namespace ExpenseTracker.Application.Jobs.Subscription.Interfaces
{
    public interface ISubscriptionJob
    {
        Task CreateSubscriptionsExpenseAsync();
        Task CreateSubscriptionsExpenseAsync(CancellationToken cancellationToken = default);
    }
}