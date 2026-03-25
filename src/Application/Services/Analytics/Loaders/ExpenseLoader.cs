using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Application.Services.Analytics.Interfaces;
using Spentir.Domain.Models.Entities;

namespace Spentir.Application.Services.Analytics.Loaders
{
    /// <summary>
    /// Loads expense data for analytics operations.
    /// Encapsulates repository access and ensures consistent filtering.
    /// </summary>

    public class ExpenseLoader(IExpenseRepository expenseRepository) : IExpenseLoader
    {
        private readonly IExpenseRepository _expenseRepository = expenseRepository;

        public async Task<List<Expense>> LoadAsync(Guid userId, DateOnly start, DateOnly end)
        {
            var (items, _) = await _expenseRepository.GetAsync(userId, start, end, page: 1, pageSize: int.MaxValue);

            return items;
        }
    }
}
