using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Model
{
    public static class WeatherCategoryExtensions
    {
        public enum WeatherCategory
        {
            Freezing,
            Chilly,
            Mild,
            Warm,
            Scorching,
            Sweltering,
            Sunny,
            Cloudy,
            Rain,
            Thunderstorm,
            Snow,
            Fog,
            Windy
        }

        private static readonly Dictionary<WeatherCategory, string> ImagePaths = new()
        {
            { WeatherCategory.Freezing, "freezing.png" },
            { WeatherCategory.Chilly, "chilly.png" },
            { WeatherCategory.Mild, "mild.png" },
            { WeatherCategory.Warm, "warm.png" },
            { WeatherCategory.Scorching, "heat_wave.png" },
            { WeatherCategory.Sweltering, "heat_wave.png" },
            { WeatherCategory.Sunny, "sunny.png" },
            { WeatherCategory.Cloudy, "cloudy.png" },
            { WeatherCategory.Rain, "rain.png" },
            { WeatherCategory.Thunderstorm, "thunderstorm.png" },
            { WeatherCategory.Snow, "snow.png" },
            { WeatherCategory.Fog, "fog.png" },
            { WeatherCategory.Windy, "windy.png" }
        };
        public static string GetImagePath(this WeatherCategory category)
        {
            return ImagePaths.TryGetValue(category, out var path) ? path : "Images/weather.png";
        }

        public static string GetImagePath(string summary)
        {
            if (Enum.TryParse<WeatherCategory>(summary, true, out WeatherCategory category))
            {
                return ImagePaths.TryGetValue(category, out var path) ? path : "weather.png";
            }
            return "weather.png";
        }

        public static string GetRandomSummary()
        {
            var values = Enum.GetValues<WeatherCategory>();
            var randomCategory = values[Random.Shared.Next(values.Length)];
            return randomCategory.ToString();
        }
    }
}
