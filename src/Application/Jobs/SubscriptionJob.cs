using Spentir.Application.Jobs.Interfaces;
using Spentir.Application.Services.Interfaces;
using Spentir.Application.DTOs;

namespace Spentir.Application.Jobs
{
    public class SubscriptionJob(ISubscriptionService subscriptionService, IServiceScopeFactory scopeFactory) : ISubscriptionJob
    {
        private readonly ISubscriptionService _subscriptionService = subscriptionService;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private const int BatchSize = 100;
        private const int MaxConcurrency = 20;
        private const int MaxRetries = 3;

        public async Task CreateSubscriptionsExpense()
        {
            var subscriptions = await _subscriptionService.GetJobSubscriptionAsync(true);

            var batches = subscriptions.Chunk(BatchSize);

            foreach (var batch in batches) await ProcessBatchAsync(batch);
        }

        private async Task ProcessBatchAsync(IEnumerable<SubscriptionDto> batch)
        {
            using var throttler = new SemaphoreSlim(MaxConcurrency);
            var tasks = batch.Select(s => ProcessSubscriptionWithRetryAsync(s, throttler));
            await Task.WhenAll(tasks);
        }

        private async Task ProcessSubscriptionWithRetryAsync(SubscriptionDto subscription, SemaphoreSlim throttler)
        {
            await throttler.WaitAsync();

            try
            {
                var attempt = 0;

                while (true)
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();

                        var expenseService = scope.ServiceProvider.GetRequiredService<IExpenseService>();

                        if (await expenseService.ExpenseAlreadyCreatedAsync(subscription.Id, DateOnly.FromDateTime(DateTime.UtcNow)))
                            return;

                        var expense = new CreateExpenseDto
                        {
                            Category = subscription.Category,
                            Amount = subscription.Amount,
                        };

                        await expenseService.CreateExpenseAsync(expense, subscription.UserId);
                        return;
                    }
                    catch
                    {
                        attempt++;

                        if (attempt >= MaxRetries)
                            throw;

                        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
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
