using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.Application.Services.Analytics.Interfaces;
using ExpenseTracker.API.Controllers.Base;
using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.API.Controllers
{
    // [Authorize]
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/analytics")]
    public class AnalyticsController(IAnalyticService analyticsService) : BaseController
    {
        private readonly IAnalyticService _analyticService = analyticsService;

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("month")]
        public async Task<IActionResult> GetMonthAnalytics(DateOnly date)
        {
            // var analyticsDto = await _analyticService.GetMonthAnalyticsAsync(date, Guid.Parse(UserId));
            var analyticsDto = await _analyticService.GetMonthAnalyticsAsync(date, Guid.Parse("3ba6b5b1-60d6-4179-9cae-9ba4bed6a39b"));

            return Ok(analyticsDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("year")]
        public async Task<IActionResult> GetYearAnalytics(DateOnly date)
        {
            // var analyticsDto = await _analyticService.GetYearAnalyticsAsync(date, Guid.Parse(UserId));
            var analyticsDto = await _analyticService.GetYearAnalyticsAsync(date, Guid.Parse("3ba6b5b1-60d6-4179-9cae-9ba4bed6a39b"));

            return Ok(analyticsDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("category")]
        public async Task<IActionResult> GetCategoryTrendAnalytics(ExpenseCategory category, DateOnly date, int range)
        {
            // var analyticsDto = await _analyticService.GetCategoryTrendAsync(category, date, range, Guid.Parse(UserId));
            var analyticsDto = await _analyticService.GetCategoryTrendAsync(category, date, range, Guid.Parse("3ba6b5b1-60d6-4179-9cae-9ba4bed6a39b"));

            return Ok(analyticsDto);
        }
    }
}