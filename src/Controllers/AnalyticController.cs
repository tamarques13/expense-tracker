using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Spentir.Services.Interfaces;
using Spentir.Controllers.Base;
using Spentir.Models;

namespace Spentir.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/analytics")]
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
            var analyticsDto = await _analyticService.GetMonthAnalyticsAsync(date, Guid.Parse(UserId));

            return Ok(analyticsDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("year")]
        public async Task<IActionResult> GetYearAnalytics(DateOnly date)
        {
            var analyticsDto = await _analyticService.GetYearAnalyticsAsync(date, Guid.Parse(UserId));

            return Ok(analyticsDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("category")]
        public async Task<IActionResult> GetCategoryTrendAnalytics(ExpenseCategory category, DateOnly date, int range)
        {
            var analyticsDto = await _analyticService.GetCategoryTrendAsync(category, date, range, Guid.Parse(UserId));

            return Ok(analyticsDto);
        }
    }
}