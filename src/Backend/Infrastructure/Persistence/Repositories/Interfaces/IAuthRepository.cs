using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
    } 
}