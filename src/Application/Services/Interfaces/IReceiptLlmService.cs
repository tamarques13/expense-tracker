using Spentir.Application.DTOs;
using OpenAI.Chat;
using System.Text.Json;
namespace Spentir.Application.Services.Interfaces
{
    public interface IReceiptLlmService
    {
        Task<JsonDocument> StructureReceiptAsync(string ocrText, CancellationToken cancellationToken);
    }

}