using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Jobs.Subscription.Interfaces;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Infrastructure.Persistence.Transactions.Interfaces;

namespace ExpenseTracker.Application.Jobs.Subscription
{
    /// <summary>
    /// Defines the side effect operations executed during subscription processing.
    /// 
    /// This class performs:
    /// - Subscription deactivation when expiration rules are met
    /// - Expense creation for renewals
    /// - Transactional updates to ensure consistency between expense creation and subscription state
    /// 
    /// All operations are intentionally isolated from business rule evaluation.
    /// </summary>

    public class SubscriptionActions(
        ISubscriptionService subscriptionService,
        IExpenseService expenseService,
        IUnitOfWork unitOfWork,
        ILogger<SubscriptionActions> logger) : ISubscriptionActions
    {
        private readonly ISubscriptionService _subscriptionService = subscriptionService;
        private readonly IExpenseService _expenseService = expenseService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<SubscriptionActions> _logger = logger;

        /// <summary>
        /// Deactivates a subscription that has reached its expiration date.
        /// </summary>
        /// <param name="subscription">The subscription to deactivate.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>

        public async Task DeactivateAsync(SubscriptionDto subscription, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Deactivating subscription {SubscriptionId} for user {UserId}", subscription.Id, subscription.UserId);

            await _subscriptionService.UpdateSubscriptionStateAsync(subscription.Id, subscription.UserId);
        }

        /// <summary>
        /// Creates an expense entry for a renewing subscription and updates its last generated date.
        /// The entire operation is executed within a transaction to guarantee consistency.
        /// </summary>
        /// <param name="subscription">The subscription for which the expense is being created.</param>
        /// <param name="today">The business date used for the renewal operation.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>

        public async Task CreateExpenseAsync(SubscriptionDto subscription, DateOnly today, CancellationToken cancellationToken = default)
        {
            var expense = new CreateExpenseDto
            {
                Category = subscription.Category,
                Amount = subscription.Amount,
            };

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _expenseService.CreateExpenseAsync(expense, subscription.UserId, cancellationToken);
                await _subscriptionService.UpdateLastGeneratedDateAsync(subscription.Id, today, subscription.UserId, cancellationToken);
            }, cancellationToken);

            _logger.LogInformation("Created expense for subscription {SubscriptionId} for user {UserId} on {Date}", subscription.Id, subscription.UserId, today);
        }
    }
}
