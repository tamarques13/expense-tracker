using Spentir.Application.DTOs;

namespace Spentir.Application.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto dto, Guid userId);
        Task<List<SubscriptionDto>> GetSubscriptionAsync(Guid userId);
        Task<List<SubscriptionDto>> GetJobSubscriptionAsync(bool isActive);
        Task<SubscriptionDto> GetSubscriptionByIdAsync(Guid Id, Guid userId);
        Task UpdateSubscriptionAsync(Guid Id, CreateSubscriptionDto dto, Guid userId);
        Task UpdateSubscriptionStateAsync(Guid Id, Guid userId);
        Task DeleteSubscriptionAsync(Guid Id, Guid userId);
    }
}