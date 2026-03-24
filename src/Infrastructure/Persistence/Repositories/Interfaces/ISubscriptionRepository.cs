using Spentir.Domain.Models.Entities;

namespace Spentir.Infrastructure.Persistence.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task AddAsync(Subscription subscription);
        Task UpdateAsync(Subscription subscription);
        Task<List<Subscription>> GetAllAsync(Guid userId);
        Task<List<Subscription>> GetAllForBackgroundJobAsync(bool isActive);
        Task<Subscription> GetByIdAsync(Guid Id, Guid userId);
        Task DeleteAsync(Subscription subscription);
    }
}