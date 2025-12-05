using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;
using WeatherApp.Services;

namespace WeatherApp.ViewModel
{
    public class MainPageViewModel
    {
        public readonly WeatherApiService _api;
        public ObservableCollection<AlertRecord> Alerts { get; set; } = new ObservableCollection<AlertRecord>();
        public Command<AlertRecord> OpenDetailCommand { get; }

        private List<AlertRecord> _allAlerts = new List<AlertRecord>();

        public bool IsInitialized { get; private set; }

        public MainPageViewModel()
        {
            _api = new WeatherApiService();
            OpenDetailCommand = new Command<AlertRecord>(OpenDetail);
        }

        public async Task InitializeAsync()
        {
            if (IsInitialized) return;
            IsInitialized = true;

            await LoadUserSpecificAlarmsAsync();
        }

        private async void OpenDetail(AlertRecord alert)
        {
            if (alert == null)
                return;

            await Application.Current.MainPage.Navigation.PushAsync(new AlertDetailPage(alert));
        }

        private async Task LoadUserSpecificAlarmsAsync()
        {
            try
            {
                Alerts.Clear();

                _allAlerts = await _api.GetUserSpecificAlertsAsync();
                foreach (AlertRecord alert in _allAlerts)
                {
                    // Pokud API neposílá ikonku — přiřadíme podle severity
                    alert.Icon ??= "weather.png";
                    Alerts.Add(alert);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API error: {ex}");
            }
        }
    }
}
