using Microsoft.EntityFrameworkCore;
using Spentir.Domain.Models.Entities;

namespace Spentir.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Primary EF Core database context for the Spentir application.
    /// 
    /// This context:
    /// - Defines the entity sets used by the application
    /// - Applies relational mappings and constraints
    /// - Configures indexes and delete behaviors that enforce domain rules
    /// 
    /// It represents the persistence boundary for all financial and subscription data.
    /// </summary>

    public class SpentirDbContext(DbContextOptions<SpentirDbContext> options) : DbContext(options)
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
            .HasOne(e => e.User)
            .WithMany(e => e.Expenses)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subscription>()
            .HasOne(e => e.User)
            .WithMany(e => e.Subscriptions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
           .HasIndex(r => new { r.UserId, r.CreatedAt });

            modelBuilder.Entity<Subscription>()
            .HasIndex(r => new { r.UserId, r.IsActive, r.ExpireDay, r.LastGenerated });
        }
    }
}