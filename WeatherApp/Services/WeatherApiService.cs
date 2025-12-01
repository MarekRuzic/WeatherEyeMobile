using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;

namespace WeatherApp.Services
{
    public class WeatherApiService
    {
        private readonly HttpClient _httpClient;

        public WeatherApiService()
        {
            _httpClient = ApiClient.httpClient;
        }

        public async Task<List<AlertRecord>> GetAlertsAsync(string region, string language = "cs")
        {
            try
            {
                string url = $"https://api.weathereye.eu/CAP/Alarms?language={language}&region={region}";

                var result = await _httpClient.GetFromJsonAsync<List<AlertRecord>>(url);

                if (result == null)
                    return new List<AlertRecord>();
                return result;
            }
            catch
            {
                return new List<AlertRecord>();
            }
        }
    }
}
