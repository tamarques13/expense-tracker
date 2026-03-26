using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Application.Services.Auth.Interfaces;
using Spentir.Domain.Models.Entities;
using Spentir.Domain.Exceptions;

namespace Spentir.Application.Services.Auth.Tokens
{
    public class AuthToken(IRefreshTokenRepository refreshTokenRepository) : IAuthToken
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        public async Task RevokeAllTokensForUser(Guid userId)
        {
            var tokens = await _refreshTokenRepository.GetAsync(userId);

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            await _refreshTokenRepository.SaveChangesAsync();
        }

        public async Task ValidateRefreshTokenAsync(RefreshToken oldToken)
        {
            if (oldToken.ExpireDate <= DateTime.UtcNow)
            {
                oldToken.IsRevoked = true;
                oldToken.RevokedAt = DateTime.UtcNow;
                await _refreshTokenRepository.UpdateAsync(oldToken);

                throw new UnauthorizedAccessException("Refresh token expired. Please log in again.");
            }

            if (oldToken.IsRevoked && oldToken.ReplacedByToken != null)
            {
                await RevokeAllTokensForUser(oldToken.UserId);
                throw new SecurityException("Refresh token reuse detected. Please log in again.");
            }
        }
    }
}