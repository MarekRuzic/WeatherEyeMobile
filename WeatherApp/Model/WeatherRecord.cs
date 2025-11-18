using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Model
{
    public class WeatherRecord
    {
        public DateTime Date { get; set; }
        public double TemperatureC { get; set; }
        public double TemperatureF => 32 + (TemperatureC / 0.5556);
        public string Summary { get; set; }
        public string Icon { get; set; }
    }
}
