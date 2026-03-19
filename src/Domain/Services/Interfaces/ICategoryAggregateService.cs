using Spentir.Models;
using Spentir.Domain.Models;

namespace Spentir.Domain.Services.Interfaces
{
    public interface ICategoryAggregateService
    {
        Dictionary<ExpenseCategory, CategoryAggregate> GroupByCategory(IEnumerable<Expense> expenses);
        Dictionary<ExpenseCategory, decimal> GroupByCategorySimple(IEnumerable<Expense> expenses);
        Dictionary<(int Year, int Month), decimal> GroupExpensesByMonth(List<Expense> expenses);
    }
}
