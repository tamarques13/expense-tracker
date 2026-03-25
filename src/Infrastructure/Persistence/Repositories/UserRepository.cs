using Microsoft.EntityFrameworkCore;
using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Infrastructure.Persistence.Configurations;
using Spentir.Domain.Models.Entities;

namespace Spentir.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository responsible for managing <see cref="User"/> persistence.
    /// </summary>
    /// 
    public class UserRepository(SpentirDbContext context) : IUserRepository
    {
        private readonly SpentirDbContext _context = context;

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}