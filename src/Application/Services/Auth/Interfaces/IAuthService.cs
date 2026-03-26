using Spentir.Application.DTOs;

namespace Spentir.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<UserTokenDto> CreateUserAsync(CreateUserDto dto, string ipAddress);
        Task<UserTokenDto> LoginUserAsync(LoginUserDto dto, string ipAddress);
        Task<UserTokenDto> RotateRefreshTokenAsync(string Token, string ipAddress);
        Task LogOutAsync(Guid userId);
    }
}