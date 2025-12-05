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
    public class PreferencesViewModel : INotifyPropertyChanged
    {
        private readonly UserApiService _userApiService = new UserApiService();
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<UserPreferenceModel> Preferences { get; set; } = new ObservableCollection<UserPreferenceModel>();

        public List<UserPreferenceModel> UserPreferences = new List<UserPreferenceModel>();


        public PreferencesViewModel()
        {
            LoadUserPreferencesAsync();
        }

        private async void LoadUserPreferencesAsync()
        {
            UserPreferences.Clear();
            UserPreferences = await _userApiService.GetPreferencesAsync();
            LoadData(UserPreferences);
        }

        private void LoadData(List<UserPreferenceModel> userPreferences)
        {
            Preferences.Clear();
            foreach (var preference in userPreferences)
            {
                Preferences.Add(preference);
            }            
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
