using Spentir.Domain.Exceptions;

namespace Spentir.Domain.Models.Entities
{
    public enum ExpenseCategory { Food, Groceries, Transport, Housing, Utilities, Health, Entertainment, Shopping, Other }
    public class Expense
    {
        public Expense() { }
        public Guid Id { get; private set; }
        public ExpenseCategory Category { get; private set; }
        public decimal Amount { get; private set; }
        public DateOnly CreatedAt { get; private set; }
        public Guid UserId { get; private set; }
        public User? User { get; private set; }

        public Expense(ExpenseCategory category, decimal value, Guid userId)
        {
            if (!Enum.IsDefined(typeof(ExpenseCategory), category))
                throw new DomainException($"Invalid resource type: {category}");
            if (value <= 0)
                throw new DomainException("Amount must be greater than zero");

            Id = Guid.NewGuid();
            Category = category;
            Amount = value;
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            UserId = userId;
        }

        public void Update(ExpenseCategory category, decimal value)
        {
            if (!Enum.IsDefined(typeof(ExpenseCategory), category))
                throw new Exception($"Invalid resource type: {category}");
            if (value <= 0)
                throw new Exception("Amount must be greater than zero");

            Category = category;
            Amount = value;
        }
    }
}