using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace WeatherApp
{
    public class FirebaseToken
    {
        public string token { get; set; }

        public FirebaseToken() { }

        public FirebaseToken(string token)
        {
            this.token = token;
        }
    }
}
