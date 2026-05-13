using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Domain.Models.Entities;
using ExpenseTracker.Infrastructure.Persistence.Repositories.Interfaces;
using ExpenseTracker.Infrastructure.Persistence.Configurations;

namespace ExpenseTracker.Infrastructure.Persistence.Repositories
{

    public class RefreshTokenRepository(ExpenseDbContext context) : IRefreshTokenRepository
    {
        private readonly ExpenseDbContext _context = context;


        public async Task AddAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);

            await _context.SaveChangesAsync();
        }

        public async Task<List<RefreshToken>> GetAsync(Guid userId)
        {
            return await _context.RefreshTokens.Where(t => t.UserId == userId && !t.IsRevoked).ToListAsync();
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RefreshToken token)
        {
            _context.RefreshTokens.Remove(token);

            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}