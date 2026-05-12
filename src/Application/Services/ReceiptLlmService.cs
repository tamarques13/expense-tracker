using OpenAI;
using OpenAI.Chat;
using ExpenseTracker.Application.Services.Interfaces;
using System.Text.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace ExpenseTracker.Application.Services
{
    public sealed class ReceiptLlmService : IReceiptLlmService
    {
        public async Task<JsonDocument> StructureReceiptAsync(Stream imageStream, CancellationToken cancellationToken)
        {
            ChatClient client = new(
                model: "gpt-4o-mini",
                apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            );

            byte[] processed = PreprocessImage(imageStream);
            BinaryData data = new(processed);

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(SystemPrompt),
                new UserChatMessage([
                        ChatMessageContentPart.CreateTextPart("Describe this image:"),
                        ChatMessageContentPart.CreateImagePart(data, "image/png", ChatImageDetailLevel.Low)
                    ])
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

CATEGORY RULES:
- ""Food"" = restaurants, cafés, bars
- ""Groceries"" = supermarkets, mini-markets
- If unclear, choose ""Other""

TOTAL RULES:
- ""total"" must be a number.
- If total is missing, compute it ONLY from item lines that clearly contain prices.
- If no reliable total can be computed, return null.

OUTPUT FORMAT:
{
  ""category"": """",
  ""total"": 0
}

IMPORTANT:
Return ONLY valid JSON.
Do NOT wrap the JSON in backticks.
Do NOT include explanations.
Do NOT include markdown.
Do NOT include any text before or after the JSON.
";

        private static byte[] PreprocessImage(Stream input)
        {
            using var image = Image.Load<Rgba32>(input);

            // 1. Convert to grayscale
            image.Mutate(x => x.Grayscale());

            // 2. Resize to max 1024px (preserve aspect ratio)
            const int maxDim = 1024;
            if (image.Width > maxDim || image.Height > maxDim)
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(maxDim, maxDim)
                }));
            }

            // 3. Save to PNG bytes
            using var ms = new MemoryStream();
            image.Save(ms, new PngEncoder());
            return ms.ToArray();
        }
    }
}