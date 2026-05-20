using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Jobs.Subscription.Interfaces;

namespace ExpenseTracker.Application.Jobs.Subscription
{
    /// <summary>
    /// Coordinates the execution of subscription business rules and actions.
    /// 
    /// This processor:
    /// - Evaluates rule outcomes in the correct order (expiration → duplication → renewal)
    /// - Delegates side effects to the actions layer
    /// - Ensures each subscription is processed deterministically and idempotently
    /// 
    /// It contains no persistence logic and no domain decisions of its own.
    /// </summary>

    public class SubscriptionProcessor(ISubscriptionRules rules, ISubscriptionActions actions, ILogger<SubscriptionProcessor> logger) : ISubscriptionProcessor
    {
        private readonly ISubscriptionRules _rules = rules;
        private readonly ISubscriptionActions _actions = actions;
        private readonly ILogger<SubscriptionProcessor> _logger = logger;

        /// <summary>
        /// Executes the full processing pipeline for a single subscription.
        /// The processor evaluates business rules in sequence and triggers the
        /// appropriate action based on the first matching condition.
        /// </summary>
        /// <param name="subscription">The subscription being processed.</param>
        /// <param name="today">The business date used for rule evaluation.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>

        public async Task ProcessAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Processing subscription {SubscriptionId} for user {UserId} on {Date}", subscription.Id, subscription.UserId, today);

            if (await _rules.ShouldExpireAsync(subscription, today, cancellationToken))
            {
                _logger.LogDebug("Subscription {SubscriptionId} should expire on {Date}. Deactivating.", subscription.Id, today);
                await _actions.DeactivateAsync(subscription, cancellationToken);

                return;
            }

            if (await _rules.ExpenseAlreadyExistsAsync(subscription, today, cancellationToken))
            {
                _logger.LogDebug("Expense already exists for subscription {SubscriptionId} on {Date}. Skipping.", subscription.Id, today);

                return;
            }

            if (!await _rules.ShouldRenewAsync(subscription, today, cancellationToken))
            {
                _logger.LogDebug("Today is not renew day for subscription {SubscriptionId}. Skipping.", subscription.Id);

                return;
            }

            await _actions.CreateExpenseAsync(subscription, today, cancellationToken);
        }
    }
}
