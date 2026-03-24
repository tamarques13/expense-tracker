using Spentir.Application.DTOs;

namespace Spentir.Application.Jobs.Subscription.Interfaces
{
    public interface ISubscriptionRules
    {
        Task<bool> ShouldExpireAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default);
        Task<bool> ExpenseAlreadyExistsAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default);
        Task<bool> ShouldRenewAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default);
    }
}
