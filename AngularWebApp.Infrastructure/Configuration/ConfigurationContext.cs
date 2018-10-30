using Microsoft.EntityFrameworkCore;

namespace AngularWebApp.Infrastructure.Configuration
{
    public class ConfigurationContext : DbContext
    {
        public ConfigurationContext(DbContextOptions options) 
            : base(options)
        {

        }

        public DbSet<ConfigurationValue> Configuration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationValue>()
                .HasKey(c => new { c.Area, c.Key });
        }
    }
}