using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Services
{
    public class ApiClient
    {
        public static readonly HttpClient httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.weathereye.eu/"),
            Timeout = TimeSpan.FromSeconds(30)
        };

        public void Help()
        {

        }
    }
}
