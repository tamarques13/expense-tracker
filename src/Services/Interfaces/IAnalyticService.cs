using Spentir.Models;
using Spentir.DTOs;

namespace Spentir.Services.Interfaces
{
    public interface IAnalyticService
    {
        Task<MonthAnalyticsDto> GetMonthAnalyticsAsync(DateOnly? date, Guid userId);
        Task<YearAnalyticsDto> GetYearAnalyticsAsync(DateOnly? date, Guid userId);
        Task<CategoryTrendDto> GetCategoryTrendAsync(ExpenseCategory category, DateOnly? date, Guid userId);

    }
}