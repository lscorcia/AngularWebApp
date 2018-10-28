using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AngularWebApp.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace AngularWebApp.Web.Authentication
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}