using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Application.Services.Interfaces;
using Spentir.Domain.Models.Entities;
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

            return new SubscriptionDto
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Category = subscription.Category.ToString(),
                Amount = subscription.Amount,
                RenewDay = subscription.RenewDay,
                ExpireDay = subscription.ExpireDay,
                IsActive = subscription.IsActive,
                UserId = subscription.UserId
            };
        }

        public async Task<List<SubscriptionDto>> GetSubscriptionAsync(Guid userId)
        {
            var subscriptions = await _subscriptionRepository.GetAsync(userId);

            var subscritpionsDto = new List<SubscriptionDto>();

            foreach (var s in subscriptions)
            {
                subscritpionsDto.Add(new SubscriptionDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category.ToString(),
                    Amount = s.Amount,
                    RenewDay = s.RenewDay,
                    ExpireDay = s.ExpireDay,
                    IsActive = s.IsActive,
                    UserId = s.UserId
                });
            }

            return subscritpionsDto;
        }

        public async Task<SubscriptionDto> GetSubscriptionByIdAsync(Guid Id, Guid userId)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(Id, userId);

            return new SubscriptionDto
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Category = subscription.Category.ToString(),
                Amount = subscription.Amount,
                RenewDay = subscription.RenewDay,
                ExpireDay = subscription.ExpireDay,
                IsActive = subscription.IsActive,
                UserId = subscription.UserId
            };
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