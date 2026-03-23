using Spentir.Domain.Models.Entities;

namespace Spentir.Application.DTOs
{
    public class SubscriptionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly RenewDay { get; set; }
        public DateOnly? ExpireDay { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? LastGenerated { get; set; }
        public Guid UserId { get; set; }
    }

    public class CreateSubscriptionDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly RenewDay { get; set; }
        public DateOnly? ExpireDay { get; set; }
    }
}