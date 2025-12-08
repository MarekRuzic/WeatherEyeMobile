using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        
        private bool _nothingLabelVisible = true;
        public bool NothingLabelVisible
        {
            get => _nothingLabelVisible;
            set { _nothingLabelVisible = value; OnPropertyChanged(nameof(NothingLabelVisible)); }
        }
        
        private bool _alertsIndicatorIsVisible = false;
        public bool AlertsIndicatorIsVisible
        {
            get => _alertsIndicatorIsVisible;
            set { _alertsIndicatorIsVisible = value; OnPropertyChanged(nameof(AlertsIndicatorIsVisible)); }
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
                
                if (_allAlerts.Count <= 0)  NothingLabelVisible = true;
                AlertsIndicatorIsVisible = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API error: {ex}");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
