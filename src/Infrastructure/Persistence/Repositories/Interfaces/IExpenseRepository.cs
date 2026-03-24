using Spentir.Domain.Models.Entities;

namespace Spentir.Infrastructure.Persistence.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task AddAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task<List<Expense>> GetAsync(Guid userId, DateOnly? start, DateOnly? end);
        Task<Expense> GetByIdAsync(Guid expenseId, Guid userId);
        Task<bool> ExistsForSubscriptionOnDateAsync(Guid id, DateOnly date);
        Task DeleteAsync(Expense expense);
    }
}