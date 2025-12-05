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
        public ObservableCollection<string> Regions { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> RegionsSpecific { get; set; } = new ObservableCollection<string>();

        public Command<AlertRecord> OpenDetailCommand { get; }
        public Command LoadAlertsCommand { get; }
        public Command LoadMoreCommand { get; }
        public Command RefreshCommand { get; }

        private List<AlertRecord> _allAlerts = new List<AlertRecord>();

        private int _loadSize = 10;
        private int _loadedCount = 0;

        // Slouží pro změny v UI
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Stavy pro UI, aby se uživateli zobrazovali načítací kolečko a tlačítko
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

        private bool _moreAlertsIndicatorIsVisible = false;
        public bool MoreAlertsIndicatorIsVisible
        {
            get => _moreAlertsIndicatorIsVisible;
            set { _moreAlertsIndicatorIsVisible = value; OnPropertyChanged(nameof(MoreAlertsIndicatorIsVisible)); }
        }

        // VYBRANÝ REGION + ukládání preferencí
        private string _selectedRegion;
        public string SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                if (_selectedRegion == value)
                    return;

                _selectedRegion = value;
                OnPropertyChanged(nameof(SelectedRegion));

                // uložíme uživatelskou volbu
                if (!string.IsNullOrEmpty(value))
                    Preferences.Set("SelectedRegion", value);

                AlertsIndicatorIsVisible = true;
                CanLoadMore = false;

                _ = LoadAlertsAsync();
                _ = LoadRegionsSpecificAsync(_selectedRegion);
            }
        }

        private string _selectedRegionSpecific;
        public string SelectedRegionSpecific
        {
            get => _selectedRegionSpecific;
            set
            {
                if (_selectedRegionSpecific != value)
                {
                    _selectedRegionSpecific = value;
                    OnPropertyChanged(nameof(SelectedRegionSpecific));

                    AlertsIndicatorIsVisible = true;
                    CanLoadMore = false;
                    
                    LoadSpecificArea(
                        _allAlerts.FindAll(a => !string.IsNullOrWhiteSpace(a.SpecificAreaDesc) &&
                                                string.Equals(a.SpecificAreaDesc.Trim(),
                                                SelectedRegionSpecific?.Trim(),
                                                StringComparison.OrdinalIgnoreCase)
                        )
                    );
                }
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public bool IsInitialized { get; private set; } = false;


        public AlertViewModel()
        {
            _api = new WeatherApiService();

            OpenDetailCommand = new Command<AlertRecord>(OpenDetail);
            LoadAlertsCommand = new Command(async () => await LoadAlertsAsync());
            LoadMoreCommand = new Command(async () => await LoadMoreWithActivityIndicator());
            RefreshCommand = new Command(async () => 
            {
                if (!IsRefreshing) 
                    await RefreshAsync();
            });            
        }

        public async Task InitializeAsync()
        {
            if (IsInitialized) return;
            IsInitialized = true;

            await LoadRegionsAsync();
            await LoadAlertsAsync();
        }       

        private async Task RefreshAsync()
        {
            if (IsRefreshing)
                return;

            try
            {
                IsRefreshing = true;
                AlertsIndicatorIsVisible = true;
                await Task.Delay(100);

                // Reset specifické oblasti
                SelectedRegionSpecific = null;

                // Znovu načtení alertů pro vybraný kraj
                 await LoadAlertsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Refresh error: " + ex);
            }
            finally
            {
                IsRefreshing = false;
                AlertsIndicatorIsVisible = false;
            }
        }

        private async Task LoadRegionsAsync()
        {
            try
            {
                List<string> regions = await _api.GetAvailableRegionsAsync();
                Regions.Clear();

                foreach (string region in regions)
                {
                    Regions.Add(region);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Region load error {ex}");
            }
        }

        private async Task LoadRegionsSpecificAsync(string selectedRegion)
        {            
            try
            {
                List<string> regionsSpecific = await _api.GetAvailableRegionsSpecificAsync(selectedRegion);
                RegionsSpecific.Clear();

                foreach (string region in regionsSpecific)
                {
                    RegionsSpecific.Add(region);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Regions specific: {ex}");
            }
        }



        private async Task LoadAlertsAsync()
        {
            try
            {
                Alerts.Clear();
                _loadedCount = 0;

                _allAlerts = await _api.GetAlertsAsync(_selectedRegion);

                foreach (AlertRecord alert in _allAlerts)
                {
                    // Pokud API neposílá ikonku — přiřadíme podle severity
                    alert.Icon ??= "weather.png";
                }

                LoadMore();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API error: {ex}");
            }
        }

        private async Task LoadMoreWithActivityIndicator()
        {
            MoreAlertsIndicatorIsVisible = true;
            CanLoadMore = false;

            await Task.Delay(100);

            LoadMore();
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
            MoreAlertsIndicatorIsVisible = false;
            CanLoadMore = _loadedCount < _allAlerts.Count;
        }

        private async void LoadSpecificArea(List<AlertRecord> alerts)
        {
            await Task.Delay(100);
            Alerts.Clear();
            foreach (AlertRecord alert in alerts)
            {
                Alerts.Add(alert);
            }

            AlertsIndicatorIsVisible = false;
            MoreAlertsIndicatorIsVisible = false;
            CanLoadMore = false;
        }

        private async void OpenDetail(AlertRecord alert)
        {
            if (alert == null)
                return;

            await Application.Current.MainPage.Navigation.PushAsync(new AlertDetailPage(alert));
        }
    }
}
