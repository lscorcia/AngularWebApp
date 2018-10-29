using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AngularWebApp.Web.Configuration
{
    public class EntityFrameworkConfigurationSource : IConfigurationSource
    {
        private readonly string _area;
        private readonly Action<DbContextOptionsBuilder> _optionsAction;

        public EntityFrameworkConfigurationSource(string area, Action<DbContextOptionsBuilder> optionsAction)
        {
            _area = area;
            _optionsAction = optionsAction;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EntityFrameworkConfigurationProvider(_area, _optionsAction);
        }
    }
}