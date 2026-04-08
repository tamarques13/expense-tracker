namespace Spentir.Application.Services.Interfaces
{
    public interface IOcrService
    {
        Task<string> ExtractTextAsync(Stream imageStream, CancellationToken cancellationToken = default);
    }

}