using Spentir.Domain.Services.Interfaces;
using Spentir.Domain.Models;
using Spentir.Models;

namespace Spentir.Domain.Services
{
    public class CategoryAggregateService : ICategoryAggregateService
    {

        /// <summary>
        /// Aggregates expenses by <see cref="ExpenseCategory"/> and computes total and average
        /// spending for each category. Produces a dictionary where each category present in the
        /// input sequence is mapped to its corresponding aggregated metrics.
        /// </summary>
        /// <param name="expenses">The collection of expenses to group and summarize.</param>

        public Dictionary<ExpenseCategory, CategoryAggregate> GroupByCategory(IEnumerable<Expense> expenses)
        {
            return expenses
              .GroupBy(e => e.Category)
              .ToDictionary(
                  g => g.Key,
                  g => new CategoryAggregate(Total: g.Sum(x => x.Amount), Transactions: g.Count(), Average: g.Average(x => x.Amount))
              );
        }

        /// <summary>
        /// Produces a simplified category aggregation containing only total spending per category.
        /// This is used for previous month comparisons where additional metrics such as averages
        /// or transaction counts are not required.
        /// </summary>
        /// <param name="expenses">The collection of expenses to aggregate.</param>

        public Dictionary<ExpenseCategory, decimal> GroupByCategorySimple(IEnumerable<Expense> expenses)
        {
            return expenses.GroupBy(e => e.Category).ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));
        }

        /// <summary>
        /// Groups all expenses by their creation year and month, producing a dictionary
        /// where each key represents a specific (Year, Month) pair and the value is the
        /// total amount spent in that period.
        /// </summary>
        /// <param name="expenses">The list of expenses to be grouped.</param>

        public Dictionary<(int Year, int Month), decimal> GroupExpensesByMonth(List<Expense> expenses)
        {
            return expenses.GroupBy(e => (e.CreatedAt.Year, e.CreatedAt.Month)).ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));
        }
    }
}