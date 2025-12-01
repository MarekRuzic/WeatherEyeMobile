using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.ModelAuth;

namespace WeatherApp.Services
{    
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        public LoginService() 
        {
            _httpClient = ApiClient.httpClient;
        }

        public async Task<AuthResponse?> LoginAsync(string username, string password)
        {
            try
            {
                string url = "https://auth.weathereye.eu/realms/WeatherEye/protocol/openid-connect/token";

                var parameters = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "password"),
                    new("client_id", "WeatherEyeWeb"),
                    new("scope", "email openid"),
                    new("username", username),
                    new("password", password)
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<AuthResponse>(json);
            }
            catch (TaskCanceledException)
            {
                return new AuthResponse { access_token = "TIMEOUT" };
            }
            catch
            {
                return null;
            }
        }

        public async Task<AuthResponse?> RefreshTokenAsync(string refreshToken)
        {
            string url = "http://auth.weathereye.eu/realms/WeatherEye/protocol/openid-connect/token";

            var parameters = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "refresh_token"),
                new("client_id", "WeatherEyeWeb"),
                new("refresh_token", refreshToken)
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AuthResponse>(json);
        }
    }
}
