using Spentir.Domain.Services.Interfaces;
using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Application.Services.Interfaces;
using Spentir.Domain.Models.Entities;
using Spentir.Application.Mappers;
using Spentir.Application.DTOs;

namespace Spentir.Application.Services
{
    /// <summary>
    /// Application layer service responsible for managing user expenses.
    /// Coordinates domain validation, repository access and DTO mapping.
    /// This service acts as the main entry point for creating, retrieving,
    /// updating and deleting expenses within the application.
    /// </summary
    
    public class ExpenseService(IExpenseRepository expenseRepository, IDateRangeService dateRangeService) : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository = expenseRepository;
        private readonly IDateRangeService dateRangeService = dateRangeService;

        /// <summary>
        /// Creates a new expense entry for the specified user. The method validates the
        /// provided category, constructs the domain entity and persists it in the repository.
        /// </summary>
        /// <param name="dto">The data transfer object containing the expense details to create.</param>
        /// <param name="userId">The identifier of the user who owns the expense.</param>
        /// <exception cref="DomainException"> Thrown when the provided category is invalid or violates domain rules.</exception>

        public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto dto, Guid userId, CancellationToken cancellationToken = default)
        {
            var expense = new Expense(Enum.Parse<ExpenseCategory>(dto.Category), dto.Amount, userId);

            await _expenseRepository.AddAsync(expense, cancellationToken);

            return expense.ToExpenseDto();
        }

        /// <summary>
        /// Retrieves all expenses associated with the specified user. The method queries the
        /// repository and maps each domain entity to a corresponding data transfer object.
        /// </summary>
        /// <param name="userId">The identifier of the user whose expenses should be retrieved.</param>

        public async Task<ExpenseListDto> GetExpensesAsync(Guid userId, DateOnly? date, bool isLastYear, int page, int pageSize)
        {
            var targetDate = dateRangeService.Normalize(date ?? DateOnly.FromDateTime(DateTime.UtcNow));

            (DateOnly start, DateOnly end) = isLastYear ? dateRangeService.GetMonthRange(targetDate, 12) : dateRangeService.GetMonthRange(targetDate, 1);

            var (expenses, totalCount) = await _expenseRepository.GetAsync(userId, start, end, page, pageSize);

            var expensesDto = new List<ExpenseDto>();

            foreach (var e in expenses) expensesDto.Add(e.ToExpenseDto());

            return ExpenseToDto.ToListExpenseDto(expensesDto, totalCount, page, pageSize);
        }

        /// <summary>
        /// Updates an existing expense belonging to the specified user. The method loads the
        /// expense from the repository, applies the updated values and persists the changes.
        /// </summary>
        /// <param name="expenseId">The identifier of the expense to update.</param>
        /// <param name="dto">The data transfer object containing the updated expense details.</param>
        /// <param name="userId">The identifier of the user who owns the expense.</param>
        /// <exception cref="DomainException">
        /// Thrown when the expense does not exist, does not belong to the user or the updated
        /// values violate domain rules.
        /// </exception>
        /// <exception cref="KeyNotFoundException">Thrown when the expense with the specified Id is not found.</exception>

        public async Task UpdateExpenseAsync(Guid expenseId, CreateExpenseDto dto, Guid userId)
        {
            var expense = await _expenseRepository.GetByIdAsync(expenseId, userId);

            expense.Update(Enum.Parse<ExpenseCategory>(dto.Category), dto.Amount);

            await _expenseRepository.UpdateAsync(expense);
        }

        /// <summary>
        /// Deletes an existing expense belonging to the specified user. The method ensures the
        /// expense exists and is owned by the user before removing it from the repository.
        /// </summary>
        /// <param name="expenseId">The identifier of the expense to delete.</param>
        /// <param name="userId">The identifier of the user who owns the expense.</param>
        /// <exception cref="DomainException">
        /// Thrown when the expense does not exist or does not belong to the user.
        /// </exception>
        /// <exception cref="KeyNotFoundException">Thrown when the expense with the specified Id is not found.</exception>

        public async Task DeleteExpenseAsync(Guid expenseId, Guid userId)
        {
            var expense = await _expenseRepository.GetByIdAsync(expenseId, userId);

            await _expenseRepository.DeleteAsync(expense);
        }
    }
}