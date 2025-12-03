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
        public bool emailNotification { get; set; }
        public bool inAppNotification { get; set; }
    }
}
