using Plugin.Firebase.CloudMessaging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WeatherApp.Model;
using WeatherApp.ModelAuth;
using WeatherApp.Services;
using WeatherApp.ViewModel;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();      
            _viewModel = new MainPageViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!_viewModel.IsInitialized)
                await _viewModel.InitializeAsync();
        }
    }

}
