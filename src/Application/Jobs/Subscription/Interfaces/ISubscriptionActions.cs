using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Jobs.Subscription.Interfaces
{
    public interface ISubscriptionActions
    {
        Task DeactivateAsync(SubscriptionDto subscription, CancellationToken cancellationToken = default);
        Task CreateExpenseAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default);
    }
}
