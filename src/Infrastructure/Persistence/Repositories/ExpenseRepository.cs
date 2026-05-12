using ExpenseTracker.Infrastructure.Persistence.Repositories.Interfaces;
using ExpenseTracker.Infrastructure.Persistence.Configurations;
using ExpenseTracker.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository responsible for managing <see cref="Expense"/> persistence.
    /// Provides CRUD operations and paginated queries scoped to a specific user.
    /// </summary>

    public class ExpenseRepository(SpentirDbContext context) : IExpenseRepository
    {
        private readonly SpentirDbContext _context = context;

        public async Task AddAsync(Expense expense, CancellationToken cancellationToken = default)
        {
            _context.Expenses.Add(expense);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Expense expense)
        {
            _context.Expenses.Update(expense);

            await _context.SaveChangesAsync();
        }

        public async Task<(List<Expense>, int TotalCount)> GetAsync(Guid userId, DateOnly? start, DateOnly? end, int page, int pageSize)
        {
            IQueryable<Expense> query = _context.Expenses.Where(r => r.UserId == userId);

            if (start.HasValue && end.HasValue)
            {
                query = query.Where(r => r.CreatedAt >= start && r.CreatedAt < end);
            }

            var totalCount = await query.CountAsync();

            var items = await query.OrderByDescending(e => e.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }

        public async Task<Expense> GetByIdAsync(Guid expenseId, Guid userId)
        {
            return await _context.Expenses.FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId) ?? throw new KeyNotFoundException($"Expense with Id: {expenseId} not found.");
        }

        public async Task DeleteAsync(Expense expense)
        {
            _context.Expenses.Remove(expense);

            await _context.SaveChangesAsync();
        }
    }
};