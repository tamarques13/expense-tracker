using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.Application.Services.Auth.Interfaces
{
    public interface IAuthToken
    {
        Task RevokeAllTokensForUser(Guid userId);
        Task ValidateRefreshTokenAsync(RefreshToken oldToken);
    }
}