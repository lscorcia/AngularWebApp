using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using AngularWebApp.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace AngularWebApp.Web.Authentication
{
    public class AuthContext : DbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["AuthContext"].ConnectionString);
        }
    }
}