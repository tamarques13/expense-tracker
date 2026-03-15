using Microsoft.EntityFrameworkCore;
using Spentir.Models;

namespace Spentir.Data
{
    public class SpentirDbContext(DbContextOptions<SpentirDbContext> options) : DbContext(options)
    {
        public DbSet<Expense> Expenses { get; set; }
    }
}