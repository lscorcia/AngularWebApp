using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWebApp.Backend.Weather.Models;
using AngularWebApp.Backend.Weather.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularWebApp.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeatherForecastsController : ControllerBase
    {
        private WeatherService weatherService { get; }
        private readonly ILogger<WeatherForecastsController> log;

        public WeatherForecastsController(ILogger<WeatherForecastsController> _log,
            WeatherService _weatherService)
        {
            log = _log;
            weatherService = _weatherService;
        }

        [HttpGet]
        public IEnumerable<GetWeatherForecastOutputDto> List()
        {
            log.LogInformation("Retrieving weather forecasts...");
            return weatherService.GetWeatherForecasts();
        }
    }
}
