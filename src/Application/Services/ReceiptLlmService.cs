using OpenAI;
using OpenAI.Chat;
using Spentir.Application.Services.Interfaces;
using System.Text.Json;

namespace Spentir.Application.Services
{
    public sealed class ReceiptLlmService : IReceiptLlmService
    {
        public async Task<JsonDocument> StructureReceiptAsync(string ocrText, CancellationToken cancellationToken)
        {
            ChatClient client = new(
                model: "gpt-4o-mini",
                apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            );

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(SystemPrompt),
                new UserChatMessage($"OCR:\n{ocrText}")
            };

            ChatCompletion completion = await client.CompleteChatAsync(
                messages,
                options: new ChatCompletionOptions
                {
                    Temperature = 0,
                },
                cancellationToken: cancellationToken
            );
            string json = completion.Content[0].Text;

            return JsonDocument.Parse(json);
        }

        private static readonly string SystemPrompt = @"
You convert noisy OCR receipts into structured expense data.

Extract category and total. Fix obvious OCR errors. No hallucinations.

Categories:
Food, Groceries, Transport, Housing, Utilities, Health, Entertainment, Shopping, Other

Rules:
- Missing fields are null
- Total must be a number
- If total missing, compute from items

OUTPUT FORMAT (JSON ONLY), no explanations:
{
  ""category"": ""category"",
  ""total"": 0
}";
    }
}