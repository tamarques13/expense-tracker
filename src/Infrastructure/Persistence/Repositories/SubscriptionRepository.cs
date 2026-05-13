using ExpenseTracker.Infrastructure.Persistence.Configurations;
using ExpenseTracker.Infrastructure.Persistence.Repositories.Interfaces;
using ExpenseTracker.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository responsible for managing <see cref="Subscription"/> persistence.
    /// Provides CRUD operations to a specific user.
    /// </summary>

    public class SubscriptionRepository(ExpenseDbContext context) : ISubscriptionRepository
    {
        private readonly ExpenseDbContext _context = context;

        public async Task AddAsync(Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<(List<Subscription>, int TotalCount)> GetAllAsync(Guid userId, int page, int pageSize)
        {
            IQueryable<Subscription> query = _context.Subscriptions.Where(s => s.UserId == userId);

            var totalCount = await query.CountAsync();

            var items = await query.OrderByDescending(s => s.IsActive).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }

        public async Task<Subscription> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId) ?? throw new KeyNotFoundException($"Subscription with Id: {id} not found.");
        }

        public async Task DeleteAsync(Subscription subscription)
        {
            _context.Subscriptions.Remove(subscription);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Subscription>> GetAllForBackgroundJobAsync(bool isActive, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            IQueryable<Subscription> query = _context.Subscriptions.Where(s => s.IsActive == isActive);

            return await query.OrderBy(s => s.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        public async Task<bool> CheckForSubscriptionRenewDateAsync(Guid id, DateOnly date, CancellationToken cancellationToken = default)
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            return await _context.Subscriptions.AnyAsync(s => s.Id == id && (s.RenewDay.Day > daysInMonth ? daysInMonth : s.RenewDay.Day) == date.Day, cancellationToken);
        }

        public async Task<bool> CheckForSubscriptionExpireDateAsync(Guid id, DateOnly date, CancellationToken cancellationToken = default)
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            DateOnly expireDay = new(date.Year, date.Month, daysInMonth);

            return await _context.Subscriptions.AnyAsync(s => s.Id == id && s.ExpireDay != null && (s.ExpireDay > expireDay ? expireDay : s.ExpireDay) <= date, cancellationToken);
        }

        public async Task<bool> ExistsForSubscriptionOnDateAsync(Guid id, DateOnly date, Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Subscriptions.AnyAsync(s => s.Id == id && s.LastGenerated == date && s.UserId == userId, cancellationToken);
        }
    }
}