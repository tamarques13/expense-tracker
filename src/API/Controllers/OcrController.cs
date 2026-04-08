using Spentir.Application.Services.Interfaces;

namespace Spentir.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ocr")]
    public sealed class OcrController(IOcrService ocrService, IReceiptLlmService receiptLlmService) : ControllerBase
    {
        private readonly IOcrService _ocrService = ocrService;
        private readonly IReceiptLlmService _receiptLlmService = receiptLlmService;

        [HttpPost("extract")]
        public async Task<ActionResult<string>> ExtractText(IFormFile image, CancellationToken cancellationToken)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Image file is required.");

            await using var stream = image.OpenReadStream();

            var text = await _ocrService.ExtractTextAsync(stream, cancellationToken);

            return Ok(new { text });
        }

        [HttpPost("AI")]
        public async Task<ActionResult<string>> ExtractStruture([FromBody]string text, CancellationToken cancellationToken)
        {
            var value = await _receiptLlmService.StructureReceiptAsync(text, cancellationToken);

            return Ok(value);
        }
    }

}