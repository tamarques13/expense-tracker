using Spentir.Domain.Exceptions;

namespace Spentir.Domain.Models.Entities
{
    public class Subscription
    {
        public Subscription() { }
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public ExpenseCategory Category { get; private set; }
        public decimal Amount { get; private set; }
        public DateOnly RenewDay { get; private set; }
        public DateOnly? ExpireDay { get; private set; }
        public bool IsActive { get; private set; }
        public Guid UserId { get; private set; }
        public DateOnly? LastGenerated { get; private set; }
        public User? User { get; private set; }

        public Subscription(string name, ExpenseCategory category, decimal amount, DateOnly renewDay, DateOnly? expireDay, Guid userId)
        {
            if (!Enum.IsDefined(typeof(ExpenseCategory), category))
                throw new DomainException($"Invalid resource type: {category}");

            if (string.IsNullOrEmpty(name))
                throw new DomainException("User must have an FirstName");

            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Amount = amount;
            RenewDay = renewDay;
            ExpireDay = expireDay;
            IsActive = true;
            LastGenerated = null;
            UserId = userId;
        }

        public void Update(string name, ExpenseCategory category, decimal amount, DateOnly renewDay, DateOnly? expireDay)
        {
            if (!Enum.IsDefined(typeof(ExpenseCategory), category))
                throw new DomainException($"Invalid resource type: {category}");

            if (string.IsNullOrEmpty(name))
                throw new DomainException("User must have an FirstName");

            Name = name;
            Category = category;
            Amount = amount;
            RenewDay = renewDay;
            ExpireDay = expireDay;
        }

        public void ToggleSubscriptionState()
        {
            IsActive = !IsActive;
        }

        public void SetGeneratedExpenseDate(DateOnly date)
        {
            LastGenerated = date;
        }
    }
}