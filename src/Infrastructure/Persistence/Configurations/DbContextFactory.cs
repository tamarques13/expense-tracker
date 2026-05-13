using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations
{
    public class ExpenseDbContextFactory : IDesignTimeDbContextFactory<ExpenseDbContext>
    {
        public ExpenseDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set.");
            var optionsBuilder = new DbContextOptionsBuilder<ExpenseDbContext>();

            optionsBuilder.UseNpgsql(connectionString);

            return new ExpenseDbContext(optionsBuilder.Options);
        }
    }
}
