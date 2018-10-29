﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AngularWebApp.Web.Configuration
{
    public class EntityFrameworkConfigurationProvider : ConfigurationProvider
    {
        private readonly string _area;
        Action<DbContextOptionsBuilder> OptionsAction { get; }

        public EntityFrameworkConfigurationProvider(string area, Action<DbContextOptionsBuilder> optionsAction)
        {
            _area = area;
            OptionsAction = optionsAction;
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<ConfigurationContext>();
            OptionsAction(builder);

            using (var dbContext = new ConfigurationContext(builder.Options))
            {
                dbContext.Database.EnsureCreated();

                var areaConfiguration = dbContext.Configuration.Where(t => t.Area == _area);
                Data = areaConfiguration.ToDictionary(c => c.Key, c => c.Value);
            }
        }
    }
}