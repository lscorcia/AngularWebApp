using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AngularWebApp.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace AngularWebApp.Web.Authentication
{
    public class AuthContext : DbContext
    {
        private string connString { get; set; }

        public AuthContext(string connString)
        {
            this.connString = connString;
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connString);
        }
    }
}