using Spentir.Application.DTOs;

namespace Spentir.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserTokenDto> CreateUser(CreateUserDto dto);
        Task<UserTokenDto> LoginUser(LoginUserDto dto);
    }
}