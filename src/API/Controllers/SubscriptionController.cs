using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Spentir.Application.Services.Interfaces;
using Spentir.API.Controllers.Base;
using Spentir.Application.DTOs;

namespace Spentir.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/subscriptions")]
    public class SubscriptionController(ISubscriptionService subscriptionService) : BaseController
    {
        private readonly ISubscriptionService _subscriptionService = subscriptionService;

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionDto dto)
        {
            var subscriptionDto = await _subscriptionService.CreateSubscriptionAsync(dto, Guid.Parse(UserId));

            return CreatedAtAction(nameof(GetSubscriptions), subscriptionDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetSubscriptions(int page = 1, int pageSize = 20)
        {
            var subscriptionsDto = await _subscriptionService.GetSubscriptionAsync(Guid.Parse(UserId), page, pageSize);

            return Ok(subscriptionsDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("id")]
        public async Task<IActionResult> GetSubscriptions(Guid Id)
        {
            var subscriptionDto = await _subscriptionService.GetSubscriptionByIdAsync(Id, Guid.Parse(UserId));

            return Ok(subscriptionDto);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("id")]
        public async Task<IActionResult> UpdateSubscription(Guid Id, CreateSubscriptionDto dto)
        {
            await _subscriptionService.UpdateSubscriptionAsync(Id, dto, Guid.Parse(UserId));

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch("id/status")]
        public async Task<IActionResult> UpdateSubscriptionState(Guid Id)
        {
            await _subscriptionService.UpdateSubscriptionStateAsync(Id, Guid.Parse(UserId));

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteSubscription(Guid subscriptionId)
        {
            await _subscriptionService.DeleteSubscriptionAsync(subscriptionId, Guid.Parse(UserId));

            return NoContent();
        }
    }
}