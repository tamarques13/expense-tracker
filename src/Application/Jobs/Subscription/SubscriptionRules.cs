using Spentir.Application.DTOs;
using Spentir.Application.Jobs.Subscription.Interfaces;
using Spentir.Application.Services.Interfaces;

namespace Spentir.Application.Jobs.Subscription
{
    /// <summary>
    /// Encapsulates the business rule evaluations used during subscription processing.
    /// 
    /// This component:
    /// - Delegates rule checks to the subscription service
    /// - Contains no side effects or persistence logic
    /// - Ensures the processor can evaluate decisions in a clean, testable manner
    /// 
    /// Rules defined here determine *what should happen*, not *how it happens*.
    /// </summary>

    public class SubscriptionRules(ISubscriptionService subscriptionService) : ISubscriptionRules
    {
        private readonly ISubscriptionService _subscriptionService = subscriptionService;

        /// <summary>
        /// Determines whether the subscription should be deactivated based on its expiration date.
        /// </summary>
        /// <param name="subscription">The subscription being evaluated.</param>
        /// <param name="today">The business date used for rule evaluation.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>

        public Task<bool> ShouldExpireAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default)
        {
            return _subscriptionService.IsSubscriptionExpireDayAsync(subscription.Id, today, cancellationToken);
        }

        /// <summary>
        /// Determines whether an expense has already been created for the subscription on the given date.
        /// This prevents duplicate expense generation.
        /// </summary>
        /// <param name="subscription">The subscription being evaluated.</param>
        /// <param name="today">The business date used for rule evaluation.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>

        public Task<bool> ExpenseAlreadyExistsAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default)
        {
            return _subscriptionService.ExpenseAlreadyCreatedAsync(subscription.Id, today, subscription.UserId, cancellationToken);
        }

        /// <summary>
        /// Determines whether the subscription is due for renewal on the given date.
        /// </summary>
        /// <param name="subscription">The subscription being evaluated.</param>
        /// <param name="today">The business date used for rule evaluation.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        
        public Task<bool> ShouldRenewAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default)
        {
            return _subscriptionService.IsSubscriptionRenewDayAsync(subscription.Id, today, cancellationToken);
        }
    }
}
