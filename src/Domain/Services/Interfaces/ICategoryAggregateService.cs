using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

namespace Spentir.Domain.Services.Interfaces
{
    public interface ICategoryAggregateService
    {
        Dictionary<ExpenseCategory, CategoryAggregate> GroupByCategory(IEnumerable<Expense> expenses);
        Dictionary<ExpenseCategory, decimal> GroupByCategorySimple(IEnumerable<Expense> expenses);
        Dictionary<(int Year, int Month), decimal> GroupExpensesByMonth(List<Expense> expenses);
    }
}
