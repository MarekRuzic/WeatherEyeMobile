using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;

namespace WeatherApp.ViewModel
{
    public class AlertViewModel
    {
        public ObservableCollection<AlertRecord> Alerts { get; set; } = new ObservableCollection<AlertRecord>();

        public AlertViewModel()
        {
            LoadFakeAlerts();
        }

        private void LoadFakeAlerts()
        {
            Alerts.Clear();

            Alerts.Add(new AlertRecord
            {
                Event = "Žádná výstraha před teplotou",
                Severity = "Minor",
                Certainty = "Unlikely",
                Urgency = "Immediate",
                Description = "Momentálně nejsou hlášeny žádné problémy.",
                Instruction = "",
                //Area = SelectedRegion,
                Onset = DateTime.Now,
                Expires = DateTime.MinValue,
                Icon = "weather.png"
            });

            Alerts.Add(new AlertRecord
            {
                Event = "Žádná výstraha před větrem",
                Severity = "Moderate",
                Certainty = "Unlikely",
                Urgency = "Immediate",
                Description = "Vítr nepředstavuje žádné nebezpečí.",
                Instruction = "",
                //Area = SelectedRegion,
                Onset = DateTime.Now,
                Expires = DateTime.MinValue,
                Icon = "wind.png"
            });

            Alerts.Add(new AlertRecord
            {
                Event = "Žádná výstraha před větrem",
                Severity = "Severe",
                Certainty = "Unlikely",
                Urgency = "Immediate",
                Description = "Vítr nepředstavuje žádné nebezpečí.",
                Instruction = "",
                //Area = SelectedRegion,
                Onset = DateTime.Now,
                Expires = DateTime.MinValue,
                Icon = "chilly.png"
            });
        }
    }
}
