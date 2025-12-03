using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Model;

namespace WeatherApp.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;
        public UserApiService() 
        {
            this._httpClient = ApiClient.httpClient;
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

        public async Task<string?> GetUserOidAsync()
        {
            if (!await ApplyAuthHeaderAsync()) return null;

            string url = "User/UserOid";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return null;

                return JsonSerializer.Deserialize<string>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetUserOid error: " + ex);
                return null;
            }
        }

        public async Task<List<UserPreferenceModel>?> GetPreferencesAsync()
        {
            if (!await ApplyAuthHeaderAsync()) return null;

            string url = "User/Preferences";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) return null;

                return JsonSerializer.Deserialize<List<UserPreferenceModel>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPreferences error: " + ex);
                return null;
            }
        }

        public async Task<bool> SavePreferencesAsync(UserPreferenceModel model)
        {
            if (model == null) return false;
            if (!await ApplyAuthHeaderAsync()) return false;

            string url = "User/Preferences";

            try
            {
                string json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SavePreferences error: " + ex);
                return false;
            }
        }

        public async Task<bool> DeletePreferencesAsync(string preferenceOid)
        {
            if (string.IsNullOrEmpty(preferenceOid)) return false;
            if (!await ApplyAuthHeaderAsync()) return false;

            string url = $"User/Preferences?PreferenceOid={preferenceOid}";

            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeletePreferences error: " + ex);
                return false;
            }
        }

        /// <summary>
        /// POST method for saving firebase token of mobile phone for notification in database
        /// </summary>
        /// <param name="mobAppToken"></param>
        /// <returns>response.IsSuccessStatusCode from httpclient or false if there was some problem</returns>
        public async Task<bool> SaveMobAppIdAsync(string mobAppToken)
        {
            if (string.IsNullOrWhiteSpace(mobAppToken)) return false;

            if (!await ApplyAuthHeaderAsync()) return false;

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
