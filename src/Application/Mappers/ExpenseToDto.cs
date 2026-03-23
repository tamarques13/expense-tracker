using Spentir.Domain.Models.Entities;
using Spentir.Application.DTOs;

namespace Spentir.Application.Mappers
{
    public static class ExpenseToDto
    {
        public static ExpenseDto ToExpenseDto(this Expense entity)
        {
            return new ExpenseDto
            {
                Id = entity.Id,
                Category = entity.Category.ToString(),
                Amount = entity.Amount,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}