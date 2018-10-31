using System;
using System.Collections.Generic;
using System.Text;
using AngularWebApp.Infrastructure.Web.Authentication.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AngularWebApp.Infrastructure.Web.Authentication
{
    public static class InfrastructureExtensions
    {
        public static IdentityBuilder AddAuthenticationStore(this IdentityBuilder builder, string connectionString)
        {
            builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(connectionString));

            return builder.AddEntityFrameworkStores<AuthDbContext>();
        }
    }
}
