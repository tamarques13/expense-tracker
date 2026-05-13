using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Jobs.Subscription.Interfaces
{
    public interface ISubscriptionProcessor
    {
        Task ProcessAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default);
    }
}
