using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Mappers
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

        public static SubscriptionListDto ToListSubscriptionDto(List<SubscriptionDto> items, int count, int pageNumber, int pageSize)
        {
            return new SubscriptionListDto
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