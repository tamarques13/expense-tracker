using Spentir.Models;

namespace Spentir.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task AddAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task<List<Expense>> GetAsync();
        Task<Expense> GetByIdAsync(Guid expenseId);
        Task DeleteAsync(Expense expense);
    }
}