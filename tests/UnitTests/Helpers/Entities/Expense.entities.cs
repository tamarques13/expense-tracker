using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.UnitTests.Helpers.Entities
{
    public class CreateExpenseEntities
    {
        public static Expense Expense(Guid userId, ExpenseCategory category, decimal amount = 100)
        {
            return new Expense(category, amount, userId);
        }

        public static ExpenseDto ExpenseDto(Guid id, DateOnly createdAt, string category = "Food", decimal amount = 100)
        {
            return new ExpenseDto
            {
                Id = id,
                Category = category,
                Amount = amount,
                CreatedAt = createdAt
            };
        }

        public static CreateExpenseDto CreateExpenseDto(string category = "Food", decimal amount = 100)
        {
            return new CreateExpenseDto
            {
                Category = category,
                Amount = amount
            };
        }
    }
}