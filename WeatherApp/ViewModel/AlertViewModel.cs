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
    public class AlertViewModel : INotifyPropertyChanged
    {
        private readonly WeatherApiService _api;        
        public ObservableCollection<AlertRecord> Alerts { get; set; } = new ObservableCollection<AlertRecord>();

        public Command<AlertRecord> OpenDetailCommand { get; }
        public Command LoadAlertsCommand { get; }
        public Command LoadMoreCommand { get; }

        private List<AlertRecord> _allAlerts = new List<AlertRecord>();

        private int _loadSize = 20;
        private int _loadedCount = 0;

        private bool _canLoadMore;
        public bool CanLoadMore
        {
            get => _canLoadMore;
            set { _canLoadMore = value; OnPropertyChanged(nameof(CanLoadMore)); }
        }

        private bool _alertsIndicatorIsVisible = true;
        public bool AlertsIndicatorIsVisible
        {
            get => _alertsIndicatorIsVisible;
            set { _alertsIndicatorIsVisible = value; OnPropertyChanged(nameof(AlertsIndicatorIsVisible)); }
        }

        private string _selectedRegion;
        public string SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                _selectedRegion = value;
                OnPropertyChanged(nameof(SelectedRegion));

                // Kdykoli se změní region → přenačti alerty
                _ = LoadAlertsAsync();
            }
        }

        public ObservableCollection<string> Regions { get; set; }


        public AlertViewModel()
        {
            Regions = new ObservableCollection<string>
{
    "Hlavní město Praha",
    "Středočeský kraj",
    "Jihočeský kraj",
    "Plzeňský kraj",
    "Karlovarský kraj",
    "Ústecký kraj",
    "Liberecký kraj",
    "Královéhradecký kraj",
    "Pardubický kraj",
    "Kraj Vysočina",
    "Jihomoravský kraj",
    "Olomoucký kraj",
    "Zlínský kraj",
    "Moravskoslezský kraj"
};

            _api = new WeatherApiService();

            OpenDetailCommand = new Command<AlertRecord>(OpenDetail);
            LoadAlertsCommand = new Command(async () => await LoadAlertsAsync());
            LoadMoreCommand = new Command(LoadMore);

            Task.Run(async () => await LoadAlertsAsync());
        }

        private async Task LoadAlertsAsync()
        {
            try
            {
                Alerts.Clear();
                _loadedCount = 0;

                _allAlerts = await _api.GetAlertsAsync(_selectedRegion);

                foreach (AlertRecord item in _allAlerts)
                {
                    // Pokud API neposílá ikonku — přiřadíme podle severity
                    item.Icon ??= "sunny.png";
                }

                LoadMore();
            }
            catch (Exception ex)
            {
                Console.WriteLine("API error: " + ex);
            }
        }

        private void LoadMore()
        {           
            List<AlertRecord> nextItems = _allAlerts.Skip(_loadedCount).Take(_loadSize).ToList();            
            foreach (AlertRecord item in nextItems)
            {
                Alerts.Add(item);
            }
            
            _loadedCount += nextItems.Count;

            AlertsIndicatorIsVisible = false;
            CanLoadMore = _loadedCount < _allAlerts.Count;
        }

        private async void OpenDetail(AlertRecord alert)
        {
            if (alert == null)
                return;

            await Application.Current.MainPage.Navigation.PushAsync(new AlertDetailPage(alert));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
