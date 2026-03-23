using Spentir.Domain.Models.Entities;

namespace Spentir.Infrastructure.Persistence.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task AddAsync(Subscription subscription);
        Task UpdateAsync(Subscription subscription);
        Task<List<Subscription>> GetAsync(Guid userId);
        Task<Subscription> GetByIdAsync(Guid Id, Guid userId);
        Task DeleteAsync(Subscription subscription);
    }
}