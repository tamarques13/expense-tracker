using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Spentir.Infrastructure.Persistence.Configurations
{
    public class SpentirDbContextFactory : IDesignTimeDbContextFactory<SpentirDbContext>
    {
        public SpentirDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set.");
            var optionsBuilder = new DbContextOptionsBuilder<SpentirDbContext>();

            optionsBuilder.UseNpgsql(connectionString);

            return new SpentirDbContext(optionsBuilder.Options);
        }
    }
}
