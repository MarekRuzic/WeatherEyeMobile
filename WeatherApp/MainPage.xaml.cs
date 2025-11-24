using Plugin.Firebase.CloudMessaging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WeatherApp.Model;
using WeatherApp.ModelAuth;
using WeatherApp.ViewModel;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();      
            BindingContext = new WeatherListViewModel();
        }
    }

}
