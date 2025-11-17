using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class SettingsViewModel
    {
        public ObservableCollection<string> Regions { get; set; }
        public ObservableCollection<string> EventTypes { get; set; }
        public ObservableCollection<string> ThreatLevels { get; set; }

        public string SelectedRegion { get; set; }
        public string SelectedEventType { get; set; }
        public string SelectedThreatLevel { get; set; }

        public SettingsViewModel()
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
                "Vysočina",
                "Jihomoravský kraj",
                "Olomoucký kraj",
                "Zlínský kraj",
                "Moravskoslezský kraj"
            };

            EventTypes = new ObservableCollection<string>
            {
                "Silný vítr", 
                "Bouřka", 
                "Povodně", 
                "Náledí",
                "Extrémní teploty", 
                "Sněžení", 
                "Smog"
                };

            ThreatLevels = new ObservableCollection<string>
            {
                "Nízká", 
                "Střední", 
                "Vysoká"
            };
        }
    }
}
