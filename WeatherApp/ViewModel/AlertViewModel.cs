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
        public Command<AlertRecord> OpenDetailCommand { get; }


        public ObservableCollection<AlertRecord> Alerts { get; set; } = new ObservableCollection<AlertRecord>();

        public AlertViewModel()
        {
            OpenDetailCommand = new Command<AlertRecord>(OpenDetail);
            LoadFakeAlerts();
        }

        private async void OpenDetail(AlertRecord alert)
        {
            if (alert == null)
                return;

            await Application.Current.MainPage.Navigation.PushAsync(new AlertDetailPage(alert));
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
                Area = "Vysočina",
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
                Icon = "sunny.png"
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
