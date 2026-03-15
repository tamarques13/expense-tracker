using Microsoft.EntityFrameworkCore;
using Spentir.Repositories.Interfaces;
using Spentir.Data;
using Spentir.Models;

namespace Spentir.Repositories
{
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