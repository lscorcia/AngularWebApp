using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AngularWebApp.Web.Configuration
{
    public static class EntityFrameworkExtensions
    {
        public static IConfigurationBuilder AddEntityFrameworkConfig(
            this IConfigurationBuilder builder, string area, Action<DbContextOptionsBuilder> setup)
        {
            return builder.Add(new EntityFrameworkConfigurationSource(area, setup));
        }
    }
}
