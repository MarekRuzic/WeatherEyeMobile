using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.ModelAuth
{
    public class AuthStorage
    {
        public static async Task SaveTokensAsync(AuthResponse authResponse, string username)
        {
            await SecureStorage.SetAsync("access_token", authResponse.access_token);
            await SecureStorage.SetAsync("refresh_token", authResponse.refresh_token);
            await SecureStorage.SetAsync("username", username);

            Preferences.Set("token_expires", DateTime.UtcNow.AddSeconds(authResponse.expires_in));
        }

        public static Task<string> GetAccessTokenAsync() =>
            SecureStorage.GetAsync("access_token");

        public static Task<string> GetRefreshTokenAsync() =>
            SecureStorage.GetAsync("refresh_token");

        public static Task<string> GetUsername() => 
            SecureStorage.GetAsync("username");

        public static bool IsTokenExpired =>
            DateTime.UtcNow >= Preferences.Get("token_expires", DateTime.MinValue);
    }
}
