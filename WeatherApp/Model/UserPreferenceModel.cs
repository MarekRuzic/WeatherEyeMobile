using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Model
{
    public class UserPreferenceModel
    {
        public string userOid {  get; set; }
        public string preferenceOid { get; set; }
        public string areaDesc { get; set; }
        public string specificAreaDesc { get; set; }
        public int alertInfoCertainty { get; set; }
        public string alertInfoCertaintyText
        {
            get
            {
                return alertInfoCertainty switch
                {
                    0 => "Určitá",
                    1 => "Pravděpodobná",
                    2 => "Možná",
                    3 => "Nepravděpodobná",
                    4 => "Všechny",                    
                    _ => "Neznámá"
                };
            }
        }
        public int alertInfoSeverity { get; set; }
        public string alertInfoSeverityText
        {
            get
            {
                return alertInfoSeverity switch
                {
                    0 => "Extrémní",
                    1 => "Vysoká",
                    2 => "Nízká",
                    3 => "Žádná",
                    4 => "Všechny",
                    _ => "Neznámá"
                };
            }
        }


public bool emailNotification { get; set; }
        public bool inAppNotification { get; set; }
    }
}
