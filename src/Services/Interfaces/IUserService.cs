using Spentir.DTOs;

namespace Spentir.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserTokenDto> CreateUser(CreateUserDto dto);
        Task<UserTokenDto> LoginUser(LoginUserDto dto);
    }
}