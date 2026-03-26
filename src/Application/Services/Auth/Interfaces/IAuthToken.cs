using Spentir.Domain.Models.Entities;

namespace Spentir.Application.Services.Auth.Interfaces
{
    public interface IAuthToken
    {
        Task RevokeAllTokensForUser(Guid userId);
        Task ValidateRefreshTokenAsync(RefreshToken oldToken);
    }
}