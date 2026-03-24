using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Application.Services.Interfaces;
using Spentir.Domain.Models.Entities;
using Spentir.Application.Mappers;
using Spentir.Application.DTOs;

namespace Spentir.Application.Services
{
    public class SubscriptionService(ISubscriptionRepository subscriptionRepository) : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        public async Task<SubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto dto, Guid userId)
        {
            var subscription = new Subscription(dto.Name, Enum.Parse<ExpenseCategory>(dto.Category), dto.Amount, dto.RenewDay, dto.ExpireDay, userId);

            await _subscriptionRepository.AddAsync(subscription);

            return subscription.ToSubscriptionDto();
        }

        public async Task<List<SubscriptionDto>> GetSubscriptionAsync(Guid userId)
        {
            var subscriptions = await _subscriptionRepository.GetAllAsync(userId);

            var subscritpionsDto = new List<SubscriptionDto>();

            foreach (var s in subscriptions) subscritpionsDto.Add(s.ToSubscriptionDto());

            return subscritpionsDto;
        }

        public async Task<List<SubscriptionDto>> GetJobSubscriptionAsync(bool isActive)
        {
            var subscriptions = await _subscriptionRepository.GetAllForBackgroundJobAsync(isActive);

            var subscritpionsDto = new List<SubscriptionDto>();

            foreach (var s in subscriptions) subscritpionsDto.Add(s.ToSubscriptionDto());

            return subscritpionsDto;
        }

        public async Task<SubscriptionDto> GetSubscriptionByIdAsync(Guid Id, Guid userId)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(Id, userId);

            return subscription.ToSubscriptionDto();
        }

        public async Task UpdateSubscriptionAsync(Guid Id, CreateSubscriptionDto dto, Guid userId)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(Id, userId);

            subscription.Update(dto.Name, Enum.Parse<ExpenseCategory>(dto.Category), dto.Amount, dto.RenewDay, dto.ExpireDay);

            await _subscriptionRepository.UpdateAsync(subscription);
        }

        public async Task UpdateSubscriptionStateAsync(Guid Id, Guid userId)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(Id, userId);

            subscription.ToggleSubscriptionState();

            await _subscriptionRepository.UpdateAsync(subscription);
        }

        public async Task DeleteSubscriptionAsync(Guid Id, Guid userId)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(Id, userId);

            await _subscriptionRepository.DeleteAsync(subscription);
        }
    }
}