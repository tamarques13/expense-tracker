using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task AddAsync(Expense expense, CancellationToken cancellationToken = default);
        Task UpdateAsync(Expense expense);
        Task<(List<Expense>, int TotalCount)> GetAsync(Guid userId, DateOnly? start, DateOnly? end, int page, int pageSize);
        Task<Expense> GetByIdAsync(Guid expenseId, Guid userId);
        Task DeleteAsync(Expense expense);
    }
}