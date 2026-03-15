using Spentir.DTOs;

namespace Spentir.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto dto);
        Task UpdateExpenseAsync(Guid expenseId, CreateExpenseDto dto);
        Task<List<ExpenseDto>> GetExpensesAsync();
        Task DeleteExpenseAsync(Guid expenseId);

    }
}