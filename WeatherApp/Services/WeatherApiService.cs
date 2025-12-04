using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Model;
using static System.Net.WebRequestMethods;

namespace WeatherApp.Services
{
    public class WeatherApiService
    {
        private readonly HttpClient _httpClient;

        public WeatherApiService()
        {
            _httpClient = ApiClient.httpClient;
        }

        private async Task<bool> ApplyAuthHeaderAsync()
        {
            string access_token = await SecureStorage.GetAsync("access_token");

            if (string.IsNullOrEmpty(access_token))
            {
                Console.WriteLine("⚠️ Missing access_token in SecureStorage!");
                return false;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

            return true;
        }

        public async Task<List<AlertRecord>> GetAlertsAsync(string region, string language = "cs")
        {
            try
            {
                string url = $"CAP/Alarms?language={language}&region={region}";
                if (!await ApplyAuthHeaderAsync())                
                {
                    return new List<AlertRecord>();
                }

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

        public async Task<List<AlertRecord>> GetUserSpecificAlertsAsync(string language = "cs")
        {
            try
            {
                string url = $"CAP/UserSpecificAlarms?language={language}";
                var result = await _httpClient.GetFromJsonAsync<List<AlertRecord>>(url);
                if (result == null) return new List<AlertRecord>();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserSpecificAlarms error {ex}");
                return new List<AlertRecord>();
            }
        }

        public async Task<List<string>> GetAvailableRegionsAsync()
        {
            try
            {
                string url = "CAP/AvailableRegions";
                string json = await _httpClient.GetStringAsync(url);
                Console.WriteLine("API Response: " + json);

                var result = JsonSerializer.Deserialize<List<string>>(json);

                if (result == null)
                {
                    Console.WriteLine("❗️Deserializace vrátila NULL");
                    return new List<string>();
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❗️GetAvailableRegionsAsync error: " + ex);
                return new List<string>();
            }
        }

        public async Task<List<string>> GetAvailableRegionsSpecificAsync(string selectedRegion)
        {
            try
            {
                string url = $"CAP/AvailableSpecificRegions?AreaDesc={selectedRegion}";
                string json = await _httpClient.GetStringAsync(url);

                var result = JsonSerializer.Deserialize<List<string>>(json);

                if (result == null)
                {
                    return new List<string>();
                }

                return result;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }
    }
}
