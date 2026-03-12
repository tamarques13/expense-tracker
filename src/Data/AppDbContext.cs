using Microsoft.EntityFrameworkCore;

namespace Spentir.Data
{
    public class SpentirDbContext(DbContextOptions<SpentirDbContext> options) : DbContext(options)
    {
        
    }
}