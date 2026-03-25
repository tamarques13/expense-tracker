using Spentir.Domain.Models.Entities;

namespace Spentir.Application.Services.Analytics.Interfaces
{
    /// <summary>
    /// Loads expense data for analytics operations.
    /// Ensures consistent filtering and repository access.
    /// </summary>
    public interface IExpenseLoader
    {
        Task<List<Expense>> LoadAsync(Guid userId, DateOnly start, DateOnly end);
    }
}
