using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Infrastructure.Persistence.Repositories.Interfaces;
using ExpenseTracker.Infrastructure.Persistence.Configurations;
using ExpenseTracker.Domain.Models.Entities;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository responsible for managing <see cref="User"/> persistence.
    /// </summary>
    /// 
    public class AuthRepository(SpentirDbContext context) : IAuthRepository
    {
        private readonly SpentirDbContext _context = context;

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}