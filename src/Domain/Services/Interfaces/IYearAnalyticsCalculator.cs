using Spentir.Application.DTOs;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Models.ValueObjects;

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
