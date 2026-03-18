using Spentir.DTOs;

namespace Spentir.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto dto, Guid UserId);
        Task UpdateExpenseAsync(Guid expenseId, CreateExpenseDto dto, Guid UserId);
        Task<List<ExpenseDto>> GetExpensesAsync(Guid UserId, DateOnly? date, bool isLastYear);
        Task DeleteExpenseAsync(Guid expenseId, Guid UserId);

    }
}