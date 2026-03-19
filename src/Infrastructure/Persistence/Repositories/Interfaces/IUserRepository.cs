using Spentir.Domain.Models.Entities;

namespace Spentir.Infrastructure.Persistence.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
    } 
}