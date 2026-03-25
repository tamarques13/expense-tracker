using Spentir.Application.DTOs;
using Spentir.Domain.Models.Entities;

namespace Spentir.Application.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto dto, Guid userId);
        Task<List<SubscriptionDto>> GetSubscriptionAsync(Guid userId);
        Task<List<SubscriptionDto>> GetJobSubscriptionAsync(int page, int pageSize, bool isActive, CancellationToken cancellationToken = default);
        Task<SubscriptionDto> GetSubscriptionByIdAsync(Guid Id, Guid userId);
        Task UpdateSubscriptionAsync(Guid subscriptionId, CreateSubscriptionDto dto, Guid userId);
        Task UpdateSubscriptionStateAsync(Guid subscriptionId, Guid userId);
        Task UpdateLastGeneratedDateAsync(Guid subscriptionId, DateOnly date, Guid userId, CancellationToken cancellationToken = default);
        Task DeleteSubscriptionAsync(Guid subscriptionId, Guid userId);
        Task<bool> IsSubscriptionRenewDayAsync(Guid subscriptionId, DateOnly date, CancellationToken cancellationToken = default);
        Task<bool> IsSubscriptionExpireDayAsync(Guid subscriptionId, DateOnly date, CancellationToken cancellationToken = default);
        Task<bool> ExpenseAlreadyCreatedAsync(Guid subscriptionId, DateOnly date, Guid userId, CancellationToken cancellationToken = default);
    }
}