using Spentir.Application.DTOs;

namespace Spentir.Application.Jobs.Subscription.Interfaces
{
    public interface ISubscriptionProcessor
    {
        Task ProcessAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default);
    }
}
