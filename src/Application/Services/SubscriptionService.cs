using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Application.Services.Interfaces;
using Spentir.Domain.Models.Entities;
using Spentir.Application.Mappers;
using Spentir.Application.DTOs;

namespace Spentir.Application.Services
{
    /// <summary>
    /// Application layer service responsible for managing user subscriptions.
    /// Coordinates repository access, domain operations and DTO mapping.
    /// Acts as the main entry point for creating, retrieving, updating and
    /// deleting subscriptions, as well as handling renewal and expiration logic.
    /// </summary>
    
    public class SubscriptionService(ISubscriptionRepository subscriptionRepository) : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;

        /// <summary>
        /// Creates a new subscription for the specified user.
        /// </summary>
        /// <param name="dto">The data required to create the subscription.</param>
        /// <param name="userId">The identifier of the user who owns the subscription.</param>

        public async Task<SubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto dto, Guid userId)
        {
            var subscription = new Subscription(dto.Name, Enum.Parse<ExpenseCategory>(dto.Category), dto.Amount, dto.RenewDay, dto.ExpireDay, userId);

            await _subscriptionRepository.AddAsync(subscription);

            return subscription.ToSubscriptionDto();
        }

        /// <summary>
        /// Retrieves all subscriptions belonging to the specified user.
        /// </summary>
        /// <param name="userId">The identifier of the user whose subscriptions are being retrieved.</param>

        public async Task<List<SubscriptionDto>> GetSubscriptionAsync(Guid userId)
        {
            var subscriptions = await _subscriptionRepository.GetAllAsync(userId);

            var subscritpionsDto = new List<SubscriptionDto>();

            foreach (var s in subscriptions) subscritpionsDto.Add(s.ToSubscriptionDto());

            return subscritpionsDto;
        }

        /// <summary>
        /// Retrieves subscriptions intended for background job processing.
        /// </summary>
        /// <param name="isActive">Indicates whether to retrieve active or inactive subscriptions.</param>

        public async Task<List<SubscriptionDto>> GetJobSubscriptionAsync(int page, int pageSize, bool isActive, CancellationToken cancellationToken = default)
        {
            var subscriptions = await _subscriptionRepository.GetAllForBackgroundJobAsync(isActive, page, pageSize, cancellationToken);

            return subscriptions.Select(s => s.ToSubscriptionDto()).ToList();
        }

        /// <summary>
        /// Retrieves a specific subscription by its identifier for the given user.
        /// </summary>
        /// <param name="subscriptionId">The identifier of the subscription.</param>
        /// <param name="userId">The identifier of the user who owns the subscription.</param>

        public async Task<SubscriptionDto> GetSubscriptionByIdAsync(Guid subscriptionId, Guid userId)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId, userId);

            return subscription.ToSubscriptionDto();
        }

        /// <summary>
        /// Updates an existing subscription with new values.
        /// </summary>
        /// <param name="subscriptionId">The identifier of the subscription to update.</param>
        /// <param name="dto">The updated subscription data.</param>
        /// <param name="userId">The identifier of the user who owns the subscription.</param>

        public async Task UpdateSubscriptionAsync(Guid subscriptionId, CreateSubscriptionDto dto, Guid userId)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId, userId);

            subscription.Update(dto.Name, Enum.Parse<ExpenseCategory>(dto.Category), dto.Amount, dto.RenewDay, dto.ExpireDay);

            await _subscriptionRepository.UpdateAsync(subscription);
        }

        /// <summary>
        /// Toggles the active state of a subscription.
        /// </summary>
        /// <param name="subscriptionId">The identifier of the subscription.</param>
        /// <param name="userId">The identifier of the user who owns the subscription.</param>

        public async Task UpdateSubscriptionStateAsync(Guid subscriptionId, Guid userId)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId, userId);

            subscription.ToggleSubscriptionState();

            await _subscriptionRepository.UpdateAsync(subscription);
        }

        /// <summary>
        /// Deletes a subscription belonging to the specified user.
        /// </summary>
        /// <param name="subscriptionId">The identifier of the subscription to delete.</param>
        /// <param name="userId">The identifier of the user who owns the subscription.</param>

        public async Task DeleteSubscriptionAsync(Guid subscriptionId, Guid userId)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId, userId);

            await _subscriptionRepository.DeleteAsync(subscription);
        }

        /// <summary>
        /// Updates the subscription's last generated expense date.
        /// This marks the subscription as having produced its scheduled expense
        /// for the specified business date, preventing duplicate generation.
        /// </summary>
        /// <param name="subscriptionId">The identifier of the subscription being updated.</param>
        /// <param name="date">The business date to record as the last generated expense date.</param>
        /// <param name="userId">The owner of the subscription, used to enforce data access boundaries.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>

        public async Task UpdateLastGeneratedDateAsync(Guid subscriptionId, DateOnly date, Guid userId, CancellationToken cancellationToken = default)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId, userId);

            subscription.SetGeneratedExpenseDate(date);

            await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);
        }

        /// <summary>
        /// Checks whether the specified date matches the renewal day of a subscription expense.
        /// </summary>
        /// <param name="subscriptionId">The identifier of the subscription related expense.</param>
        /// <param name="date">The date to evaluate against the subscription's renewal schedule.</param>

        public async Task<bool> IsSubscriptionRenewDayAsync(Guid subscriptionId, DateOnly date, CancellationToken cancellationToken = default)
        {
            return await _subscriptionRepository.CheckForSubscriptionRenewDateAsync(subscriptionId, date, cancellationToken);
        }

        /// <summary>
        /// Determines whether the subscription has reached its configured expiration date.
        /// </summary>
        /// <param name="subscriptionId">The identifier of the subscription being evaluated.</param>
        /// <param name="date">The business date used for the expiration check.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>

        public async Task<bool> IsSubscriptionExpireDayAsync(Guid subscriptionId, DateOnly date, CancellationToken cancellationToken = default)
        {
            return await _subscriptionRepository.CheckForSubscriptionExpireDateAsync(subscriptionId, date, cancellationToken);
        }

        /// <summary>
        /// Determines whether an expense has already been created for a subscription on a specific date.
        /// </summary>
        /// <param name="subscriptionId">The identifier of the subscription being evaluated.</param>
        /// <param name="date">The date to check for an existing expense.</param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>

        public async Task<bool> ExpenseAlreadyCreatedAsync(Guid subscriptionId, DateOnly date, Guid userId, CancellationToken cancellationToken = default)
        {
            return await _subscriptionRepository.ExistsForSubscriptionOnDateAsync(subscriptionId, date, userId, cancellationToken);
        }
    }
}