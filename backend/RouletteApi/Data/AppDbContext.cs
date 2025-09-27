using Microsoft.EntityFrameworkCore;
using RouletteApi.Models;

namespace RouletteApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(x => x.Balance).HasColumnType("decimal(18,2)");
        }
    }
}
