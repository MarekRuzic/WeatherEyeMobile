using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;

namespace WeatherApp.ViewModel
{
    public class WeatherListViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<WeatherRecord> WeatherItems { get; set; } = new ObservableCollection<WeatherRecord>();

        public WeatherListViewModel()
        {
            LoadFakeData();
        }

        private void LoadFakeData()
        {
            WeatherItems.Add(new WeatherRecord
            {
                Date = new DateTime(2025, 11, 19),
                TemperatureC = 30,
                Summary = "Mild",
                Icon = "weather.png"
            });

            WeatherItems.Add(new WeatherRecord
            {
                Date = new DateTime(2025, 11, 20),
                TemperatureC = 46,
                Summary = "Chilly",
                Icon = "weather.png"
            });

            WeatherItems.Add(new WeatherRecord
            {
                Date = new DateTime(2025, 11, 21),
                TemperatureC = 14,
                Summary = "Freezing",
                Icon = "weather.png"
            });

            WeatherItems.Add(new WeatherRecord
            {
                Date = new DateTime(2025, 11, 22),
                TemperatureC = 21,
                Summary = "Mild",
                Icon = "weather.png"
            });

            WeatherItems.Add(new WeatherRecord
            {
                Date = new DateTime(2025, 11, 23),
                TemperatureC = 12,
                Summary = "Scorching",
                Icon = "weather.png"
            });

            for (int i = 0; i < 10; i++)
            {

            }
        }
    }
}
