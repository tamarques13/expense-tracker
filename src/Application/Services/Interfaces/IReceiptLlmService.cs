using System.Text.Json;
namespace ExpenseTracker.Application.Services.Interfaces
{
    public interface IReceiptLlmService
    {
        Task<JsonDocument> StructureReceiptAsync(Stream imageStream, CancellationToken cancellationToken);
    }

}