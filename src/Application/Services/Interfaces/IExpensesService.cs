using Spentir.Application.DTOs;
using Spentir.Domain.Models.Entities;

namespace Spentir.Application.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto dto, Guid UserId, CancellationToken cancellationToken = default);
        Task UpdateExpenseAsync(Guid expenseId, CreateExpenseDto dto, Guid UserId);
        Task<ExpenseListDto> GetExpensesAsync(Guid UserId, DateOnly? date, bool isLastYear, int page, int pageSize);
        Task DeleteExpenseAsync(Guid expenseId, Guid UserId);
    }
}