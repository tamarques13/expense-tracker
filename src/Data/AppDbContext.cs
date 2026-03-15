using Microsoft.EntityFrameworkCore;
using Spentir.Models;

namespace Spentir.Data
{
    public class SpentirDbContext(DbContextOptions<SpentirDbContext> options) : DbContext(options)
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
            .HasOne(e => e.User)
            .WithMany(e => e.Expenses)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
           .HasIndex(r => new { r.UserId, r.CreatedAt });
        }
    }
}