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
        Task UpdateSubscriptionAsync(Guid id, CreateSubscriptionDto dto, Guid userId);
        Task UpdateSubscriptionStateAsync(Guid id, Guid userId);
        Task UpdateLastGeneratedDateAsync(Guid id, DateOnly date, Guid userId, CancellationToken cancellationToken = default);
        Task DeleteSubscriptionAsync(Guid id, Guid userId);
        Task<bool> IsSubscriptionRenewDayAsync(Guid expenseId, DateOnly date, CancellationToken cancellationToken = default);
        Task<bool> IsSubscriptionExpireDayAsync(Guid expenseId, DateOnly date, CancellationToken cancellationToken = default);
        Task<bool> ExpenseAlreadyCreatedAsync(Guid id, DateOnly date, Guid userId, CancellationToken cancellationToken = default);
    }
}