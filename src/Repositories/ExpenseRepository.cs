using Spentir.Repositories.Interfaces;
using Spentir.Data;
using Spentir.Models;
using Microsoft.EntityFrameworkCore;

namespace Spentir.Repositories
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

        public async Task<List<Expense>> GetAsync(Guid userId)
        {
            IQueryable<Expense> query = _context.Expenses.Where(r => r.UserId == userId);

            return await query.ToListAsync();
        }

        public async Task<Expense> GetByIdAsync(Guid expenseId, Guid userId)
        {
            return _context.Expenses.FirstOrDefault(e => e.Id == expenseId && e.UserId == userId) ?? throw new KeyNotFoundException($"Expense with Id: {expenseId} not found.");
        }

        public async Task DeleteAsync(Expense expense)
        {
            _context.Expenses.Remove(expense);

            await _context.SaveChangesAsync();
        }
    }
};