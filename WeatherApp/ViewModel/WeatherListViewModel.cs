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

        private List<WeatherRecord> allItems;

        public Command LoadMoreCommand { get; }

        public WeatherListViewModel()
        {
            //LoadFakeData();

            LoadMoreCommand = new Command(LoadMore);

            // Načti všechna data (API nebo fake)
            allItems = LoadFakeDataList();

            // Poprvé zobraz jen prvních 10
            LoadMore();
        }

        private List<WeatherRecord> LoadFakeDataList()
        {
            List<WeatherRecord> items = new List<WeatherRecord>();
            for (int i = 0; i < 50; i++)
            {
                items.Add(new WeatherRecord
                {
                    Date = DateTime.Now.AddDays(-i),
                    TemperatureC = Random.Shared.Next(-10, 35),
                    Summary = WeatherCategoryExtensions.GetRandomSummary(),
                    Icon = WeatherCategoryExtensions.GetImagePath(WeatherCategoryExtensions.GetRandomSummary())
                });                
            }
            return items;
        }

        private void LoadMore()
        {
            var nextItems = allItems
                .Skip(loadedCount)
                .Take(batchSize)
                .ToList();

            foreach (var item in nextItems)
                WeatherItems.Add(item);

            loadedCount += nextItems.Count;
        }

        // Původní
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
