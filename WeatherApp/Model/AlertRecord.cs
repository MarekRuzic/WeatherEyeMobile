using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Model
{
    public class AlertRecord
    {
        public string Event { get; set; }
        public string Severity { get; set; }
        public string Certainty { get; set; }
        public string Urgency { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public string Area { get; set; }
        public DateTime Onset { get; set; }
        private DateTime? _expires;
        public DateTime? Expires
        {
            get => _expires;
            set
            {
                // Pokud je DateTime.MinValue nebo defaultní → ulož null
                if (value == null || value == DateTime.MinValue || value.Value.Year == 1)
                {
                    _expires = null;
                }
                else
                {
                    _expires = value;
                }
            }
        }

        public string Icon { get; set; }

        public string SeverityColor =>
            Severity switch
            {
                "Extreme" => "#FF0000",
                "Severe" => "#FF4500",
                "Moderate" => "#FFA500",
                "Minor" => "#4CAF50",
                _ => "#607D8B"
            };
    }
}
