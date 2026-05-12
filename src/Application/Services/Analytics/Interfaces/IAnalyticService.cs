using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Application.DTOs.Analytics;

namespace ExpenseTracker.Application.Services.Analytics.Interfaces
{
    public interface IAnalyticService
    {
        Task<MonthAnalyticsDto> GetMonthAnalyticsAsync(DateOnly? date, Guid userId);
        Task<YearAnalyticsDto> GetYearAnalyticsAsync(DateOnly? date, Guid userId);
        Task<CategoryTrendDto> GetCategoryTrendAsync(ExpenseCategory category, DateOnly? date, int range, Guid userId);
    }
}