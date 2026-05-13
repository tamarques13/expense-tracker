using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task AddAsync(Subscription subscription);
        Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default);
        Task<(List<Subscription>, int TotalCount)> GetAllAsync(Guid userId, int page, int pageSize);
        Task<List<Subscription>> GetAllForBackgroundJobAsync(bool isActive, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<Subscription> GetByIdAsync(Guid Id, Guid userId);
        Task DeleteAsync(Subscription subscription);
        Task<bool> CheckForSubscriptionRenewDateAsync(Guid id, DateOnly date, CancellationToken cancellationToken = default);
        Task<bool> CheckForSubscriptionExpireDateAsync(Guid id, DateOnly date, CancellationToken cancellationToken = default);
        Task<bool> ExistsForSubscriptionOnDateAsync(Guid id, DateOnly date, Guid userId, CancellationToken cancellationToken = default);
    }
}