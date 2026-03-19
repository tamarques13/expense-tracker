using Spentir.Domain.Models;
using Spentir.DTOs;
using Spentir.Models;

namespace Spentir.Domain.Services.Interfaces
{
    public interface IYearAnalyticsCalculator
    {
        YearAnalyticsMetrics Calculate(
            List<Expense> expenses,
            decimal totalSpent,
            List<CategoryYearAnalyticsDto> categories,
            List<YearMonthsAnalyticsDto> monthly
        );
    }
}
