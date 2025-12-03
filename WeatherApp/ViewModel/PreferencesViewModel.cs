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
    public class PreferencesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<UserPreferenceModel> Preferences { get; set; } = new ObservableCollection<UserPreferenceModel>();

        public PreferencesViewModel()
        {
            LoadFakeData();
        }

        private void LoadFakeData()
        {
            Preferences.Clear();

            Preferences.Add(new UserPreferenceModel
            {
                userOid = "user0001",
                preferenceOid = "pref001",
                areaDesc = "Praha – východ",
                emailNotification = true,
                inAppNotification = true
            });

            Preferences.Add(new UserPreferenceModel
            {
                userOid = "user0001",
                preferenceOid = "pref002",
                areaDesc = "Středočeský kraj – bouřky",
                emailNotification = false,
                inAppNotification = true
            });

            Preferences.Add(new UserPreferenceModel
            {
                userOid = "user0001",
                preferenceOid = "pref003",
                areaDesc = "Ústecký kraj – silný vítr",
                emailNotification = true,
                inAppNotification = false
            });
        }

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
