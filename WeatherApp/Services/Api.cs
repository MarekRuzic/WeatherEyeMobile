using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Services
{
    public class Api
    {
        public HttpClient Client { get; set; }

        public Api()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost/projects/API_WeatherEye/index.php/");
            //Client.BaseAddress = new Uri("http://10.0.2.2:80/projects/API_WeatherEye/index.php/");
            //Client.BaseAddress = new Uri("http://10.227.217.219/projects/API_WeatherEye/index.php/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool checkConnectivity()
        {
            return Connectivity.Current.NetworkAccess != NetworkAccess.Internet;
        }
    }
}
