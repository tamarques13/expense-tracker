using Spentir.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Spentir.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/AI")]
    public sealed class AiController(IReceiptLlmService receiptLlmService) : ControllerBase
    {
        private readonly IReceiptLlmService _receiptLlmService = receiptLlmService;

        [HttpPost("Extract")]
        public async Task<ActionResult<string>> ExtractStruture(IFormFile image, CancellationToken cancellationToken)
        {
            if (image == null || image.Length == 0) return BadRequest("Image file is required.");
            await using var stream = image.OpenReadStream();

            var value = await _receiptLlmService.StructureReceiptAsync(stream, cancellationToken);

            return Ok(value);
        }
    }
}