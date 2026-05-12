using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Mappers
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

        public static ExpenseListDto ToListExpenseDto(List<ExpenseDto> items, int count, int pageNumber, int pageSize)
        {
            return new ExpenseListDto
            {
                Items = items,
                TotalCount = count,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
        }
    }

}