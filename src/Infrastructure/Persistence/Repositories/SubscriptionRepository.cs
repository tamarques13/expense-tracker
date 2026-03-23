using Spentir.Infrastructure.Persistence.Configurations;
using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Spentir.Infrastructure.Persistence.Repositories
{
    public class SubscriptionRepository(SpentirDbContext context) : ISubscriptionRepository
    {
        private readonly SpentirDbContext _context = context;

        public async Task AddAsync(Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Subscription>> GetAsync(Guid userId)
        {
            IQueryable<Subscription> query = _context.Subscriptions.Where(s => s.UserId == userId);

            return await query.OrderByDescending(s => s.IsActive).ToListAsync();
        }

        public async Task<Subscription> GetByIdAsync(Guid Id, Guid userId)
        {
            return await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == Id && s.UserId == userId) ?? throw new KeyNotFoundException($"Subscription with Id: {Id} not found."); ;
        }

        public async Task DeleteAsync(Subscription subscription)
        {
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }
}