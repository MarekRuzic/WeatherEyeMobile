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

        private int batchSize = 15;
        private int loadedCount = 0;

        public ObservableCollection<WeatherRecord> WeatherItems { get; set; } = new ObservableCollection<WeatherRecord>();

        public WeatherListViewModel()
        {
            LoadFakeData();
        }

        private void LoadFakeData()
        {
            for (int i = 0; i < 50; i++)
            {
                WeatherItems.Add(
                    weatherRecord(
                        DateTime.Now.AddDays(-i), 
                        Random.Shared.Next(-10, 35),
                        WeatherCategoryExtensions.GetRandomSummary()
                    )
                );
            }
        }

        private WeatherRecord weatherRecord(DateTime date, double temperature, string summary)
        {
            string icon = WeatherCategoryExtensions.GetImagePath(summary);
            return new WeatherRecord
            {
                Date = date,
                TemperatureC = temperature,
                Summary = summary,
                Icon = icon
            };
        }
    }
}
