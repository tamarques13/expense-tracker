using Spentir.Models;

namespace Spentir.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task AddAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task<List<Expense>> GetAsync(Guid userId, DateOnly? start, DateOnly? end);
        Task<Expense> GetByIdAsync(Guid expenseId, Guid userId);
        Task DeleteAsync(Expense expense);
    }
}