using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace AngularWebApp.Backend.Orders.Repository
{
    class OrdersDbContext: DbContext
    {
        public string ConnectionString { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}