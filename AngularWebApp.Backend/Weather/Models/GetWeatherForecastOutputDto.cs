using System;
using System.Collections.Generic;
using System.Text;

namespace AngularWebApp.Backend.Weather.Models
{
    public class GetWeatherForecastOutputDto
    {
        public int Id { get; set; }
        public string DateFormatted { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }

        public int TemperatureF
        {
            get
            {
                return 32 + (int)(TemperatureC / 0.5556);
            }
        }
    }
}
