using Spentir.Repositories.Interfaces;
using Spentir.Services.Interfaces;
using Spentir.Models;
using Spentir.DTOs;

namespace Spentir.Services
{
    public class ExpenseService(IExpenseRepository expenseRepository) : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository = expenseRepository;
        public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseDto dto)
        {
            var expense = new Expense(Enum.Parse<ExpenseCategory>(dto.Category), dto.Amount);

            await _expenseRepository.AddAsync(expense);

            return new ExpenseDto
            {
                Id = expense.Id,
                Category = expense.Category.ToString(),
                Amount = expense.Amount
            };
        }

        public async Task<List<ExpenseDto>> GetExpensesAsync()
        {
            var expenses = await _expenseRepository.GetAsync();

            var expensesDto = new List<ExpenseDto>();

            foreach (var e in expenses)
            {
                expensesDto.Add(new ExpenseDto
                {
                    Id = e.Id,
                    Category = e.Category.ToString(),
                    Amount = Math.Round(e.Amount, 2)
                });
            }

            return expensesDto;
        }

        public async Task UpdateExpenseAsync(Guid expenseId, CreateExpenseDto dto)
        {
            var expense = await _expenseRepository.GetByIdAsync(expenseId);

            expense.Update(Enum.Parse<ExpenseCategory>(dto.Category), dto.Amount);

            await _expenseRepository.UpdateAsync(expense);
        }

        public async Task DeleteExpenseAsync(Guid expenseId)
        {
            var expense = await _expenseRepository.GetByIdAsync(expenseId);

            await _expenseRepository.DeleteAsync(expense);
        }
    }
}