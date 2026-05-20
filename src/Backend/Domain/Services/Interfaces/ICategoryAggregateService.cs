using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Domain.Models.ValueObjects;

namespace ExpenseTracker.Domain.Services.Interfaces
{
    public interface ICategoryAggregateService
    {
        Dictionary<ExpenseCategory, CategoryAggregate> GroupByCategory(IEnumerable<Expense> expenses);
        Dictionary<ExpenseCategory, decimal> GroupByCategorySimple(IEnumerable<Expense> expenses);
        Dictionary<(int Year, int Month), decimal> GroupExpensesByMonth(List<Expense> expenses);
    }
}
