using Spentir.Domain.Models.Entities;
using Spentir.Application.DTOs;

namespace Spentir.Application.Mappers
{
    public static class SubscriptionToDto
    {
        public static SubscriptionDto ToSubscriptionDto(this Subscription entity)
        {
            return new SubscriptionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Category = entity.Category.ToString(),
                Amount = entity.Amount,
                RenewDay = entity.RenewDay,
                ExpireDay = entity.ExpireDay,
                IsActive = entity.IsActive,
                LastGenerated = entity.LastGenerated,
                UserId = entity.UserId
            };
        }
    }
}