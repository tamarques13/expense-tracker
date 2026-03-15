using Spentir.Models;

namespace Spentir.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
    } 
}