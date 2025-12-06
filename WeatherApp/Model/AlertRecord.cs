using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Model
{
    public class AlertRecord
    {
        public string senderName {  get; set; }
        public string Event { get; set; }
        public int Urgency { get; set; }
        public string UrgencyText
        {
            get
            {
                return Urgency switch
                {
                    3 => "Potřeba okamžité reakce",
                    2 => "Reakce musí do hodiny",
                    1 => "Následující dny",
                    0 => "Žádná reakce",
                    _ => "Neznámá"
                };
            }
        }
        public int Severity { get; set; }
        public string SeverityText
        {
            get
            {
                return Severity switch
                {
                    0 => "Extrémní",
                    1 => "Vysoká",
                    2 => "Nízká",
                    3 => "Žádná",
                    _ => "Neznámá"
                };
            }
        }
        public string SeverityColor =>
            Severity switch
            {
                /*"Extreme" => "#FF0000",
                "Severe" => "#FF4500",
                "Moderate" => "#FFA500",
                "Minor" => "#4CAF50",
                _ => "#607D8B"*/
                0 => "#FF0000",
                1 => "#FF4500",
                2 => "#FFA500",
                3 => "#4CAF50",
                _ => "#607D8B"
            };
        public int Certainty { get; set; }
        public string CertaintyText 
        {
            get
            {
                return Certainty switch
                {
                    0 => "Určitá",
                    1 => "Pravděpodobná",
                    2 => "Možná",
                    3 => "Nepravděpodobná",
                    _ => "Neznámá"
                };
            }
        }
        public string Language { get; set; }
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
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public string AreaDesc { get; set; }

        public string SpecificAreaDesc { get; set; }

        public string Icon { get; set; }

        


    }
}
