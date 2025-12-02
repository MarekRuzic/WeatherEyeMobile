using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace WeatherApp.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;
        public UserApiService() 
        {
            this._httpClient = ApiClient.httpClient;
        }

        /// <summary>
        /// POST method for saving firebase token of mobile phone for notification in database
        /// </summary>
        /// <param name="mobAppToken"></param>
        /// <returns>response.IsSuccessStatusCode from httpclient or false if there was some problem</returns>
        public async Task<bool> SaveMobAppIdAsync(string mobAppToken)
        {
            if (string.IsNullOrWhiteSpace(mobAppToken)) return false;

            string accessToken = await SecureStorage.GetAsync("access_token");

            if (string.IsNullOrEmpty(accessToken)) return false;

            Console.WriteLine("Access token je vole: " + accessToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string url = $"User/SaveMobAppId?MobAppId={mobAppToken}";
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
