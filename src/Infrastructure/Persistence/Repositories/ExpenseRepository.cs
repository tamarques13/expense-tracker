using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Infrastructure.Persistence.Configurations;
using Spentir.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Spentir.Infrastructure.Persistence.Repositories
{
    public class ExpenseRepository(SpentirDbContext context) : IExpenseRepository
    {
        private readonly SpentirDbContext _context = context;

        public async Task AddAsync(Expense expense)
        {
            _context.Expenses.Add(expense);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expense expense)
        {
            _context.Expenses.Update(expense);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Expense>> GetAsync(Guid userId, DateOnly? start, DateOnly? end)
        {
            IQueryable<Expense> query = _context.Expenses.Where(r => r.UserId == userId);

            if (start.HasValue && end.HasValue)
            {
                query = query.Where(r => r.CreatedAt >= start && r.CreatedAt < end);
            }

            return await query.ToListAsync();
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