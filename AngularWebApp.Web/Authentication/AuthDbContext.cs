using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AngularWebApp.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AngularWebApp.Web.Authentication
{
    public class AuthDbContext : IdentityDbContext<IdentityUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}