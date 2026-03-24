using Spentir.Application.DTOs;

namespace Spentir.Application.Jobs.Subscription.Interfaces
{
    public interface ISubscriptionActions
    {
        Task DeactivateAsync(SubscriptionDto subscription, CancellationToken cancellationToken = default);
        Task CreateExpenseAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default);
    }
}
