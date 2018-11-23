using System;
using System.Collections.Generic;
using System.Linq;
using AngularWebApp.Backend.Weather.Models;
using AngularWebApp.Infrastructure.DI;
using Microsoft.Extensions.Configuration;

namespace AngularWebApp.Backend.Weather.Services
{
    public class WeatherService : IApplicationService
    {
        private IConfiguration Configuration { get; }
       
        public WeatherService(IConfiguration _config)
        {
            this.Configuration = _config;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IEnumerable<GetWeatherForecastOutputDto> GetWeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new GetWeatherForecastOutputDto
            {
                Id = index,
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
    }
}
