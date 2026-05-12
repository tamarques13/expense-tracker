using ExpenseTracker.Application.Jobs.Subscription.Interfaces;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Jobs.Subscription
{
    /// <summary>
    /// Background job responsible for processing active subscriptions in scalable batches.
    /// 
    /// This job:
    /// - Retrieves subscriptions in paginated chunks
    /// - Processes each subscription concurrently with bounded parallelism
    /// - Applies retry logic for transient failures
    /// - Executes business rules such as expiration, renewal, and expense creation
    /// - Uses isolated DI scopes to ensure thread safety and avoid shared state
    /// </summary>

    public class SubscriptionJob(ISubscriptionService subscriptionService, IServiceScopeFactory scopeFactory, ILogger<SubscriptionJob> logger) : ISubscriptionJob
    {
        private readonly ISubscriptionService _subscriptionService = subscriptionService;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<SubscriptionJob> _logger = logger;
        private const int PageSize = 200;
        private const int MaxConcurrency = 20;
        private const int MaxRetries = 3;

        /// <summary>
        /// Executes the subscription processing job without a cancellation token.
        /// </summary>

        public Task CreateSubscriptionsExpenseAsync() => CreateSubscriptionsExpenseAsync(CancellationToken.None);

        /// <summary>
        /// Iterates through all active subscriptions in paginated batches and processes each one.
        /// The job continues until all pages are exhausted or cancellation is requested.
        /// </summary>
        /// <param name="cancellationToken">Token used to request early termination of the job.</param>

        public async Task CreateSubscriptionsExpenseAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting subscription expense job at {Time}", DateTime.UtcNow);

            var page = 1;
            var today = DateOnly.FromDateTime(DateTime.Now);

            while (!cancellationToken.IsCancellationRequested)
            {
                var subscriptions = await _subscriptionService.GetJobSubscriptionAsync(page, PageSize, isActive: true, cancellationToken);

                if (subscriptions == null || subscriptions.Count == 0)
                {
                    _logger.LogInformation("No more subscriptions to process. Ending job.");
                    break;
                }

                _logger.LogInformation("Processing page {Page} with {Count} subscriptions", page, subscriptions.Count);

                await ProcessPageAsync(subscriptions, today, cancellationToken);

                page++;
            }

            _logger.LogInformation("Finished subscription expense job at {Time}", DateTime.UtcNow);
        }

        /// <summary>
        /// Processes a batch of subscriptions concurrently while enforcing a maximum concurrency limit.
        /// Each subscription is handled independently with retry support.
        /// </summary>
        /// <param name="subscriptions">The subscriptions to process in this batch.</param>
        /// <param name="today">The business date used for rule evaluation.</param>
        /// <param name="cancellationToken">Token used to cancel the batch processing.</param>
        
        private async Task ProcessPageAsync(IReadOnlyCollection<SubscriptionDto> subscriptions, DateOnly today, CancellationToken cancellationToken)
        {
            using var throttler = new SemaphoreSlim(MaxConcurrency);
            var tasks = subscriptions.Select(s => ProcessSubscriptionWithRetryAsync(s, today, throttler, cancellationToken));
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Executes processing for a single subscription with retry logic.
        /// Each attempt is isolated in its own DI scope to ensure thread safety and avoid shared state.
        /// </summary>
        /// <param name="subscription">The subscription being processed.</param>
        /// <param name="today">The business date used for rule evaluation.</param>
        /// <param name="throttler">Concurrency limiter shared across the batch.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        
        private async Task ProcessSubscriptionWithRetryAsync(SubscriptionDto subscription, DateOnly today, SemaphoreSlim throttler, CancellationToken cancellationToken)
        {
            await throttler.WaitAsync(cancellationToken);

            try
            {
                var attempt = 0;

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var processor = scope.ServiceProvider.GetRequiredService<ISubscriptionProcessor>();

                        await processor.ProcessAsync(subscription, today, cancellationToken);
                        return;
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogWarning("Processing for subscription {SubscriptionId} was cancelled.", subscription.Id);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        attempt++;
                        _logger.LogWarning(ex,
                            "Error processing subscription {SubscriptionId} on attempt {Attempt}/{MaxRetries}",
                            subscription.Id, attempt, MaxRetries);

                        if (attempt >= MaxRetries)
                        {
                            _logger.LogError(ex,
                                "Max retries reached for subscription {SubscriptionId}. Giving up.",
                                subscription.Id);
                            return;
                        }

                        var delaySeconds = Math.Pow(2, attempt);
                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken);
                    }
                }
            }
            finally
            {
                throttler.Release();
            }
        }
    }
}
