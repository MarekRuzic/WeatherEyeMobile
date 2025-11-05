using Plugin.Firebase.CloudMessaging;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            isNotificationEnable();
            App.AppResumed += (s, e) => isNotificationEnable();
        }

        protected async void isNotificationEnable()
        {
            if (DeviceInfo.Platform != DevicePlatform.Android) return;

            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
            if (status != PermissionStatus.Granted)
            {
                CheckBoxNotificationCheck.IsVisible = true;
                NoticationLineBreak.IsVisible = true;
            }
            else
            {
                CheckBoxNotificationCheck.IsVisible = false;
                NoticationLineBreak.IsVisible = false;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            isNotificationEnable();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            /*count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);*/

            await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
            var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
            Console.WriteLine($"FCM token: {token}");
        }
        
        private void OnClickedCount(object sender, EventArgs e)
        {
            DisplayAlert("Ahoj", "Ahoj", "Ok");
        }

        bool isHandlingCheckChange = false;
        private async void CheckChange_NotificationEnable(object sender, EventArgs e)
        {
            if (isHandlingCheckChange)
                return;

            isHandlingCheckChange = true;

            if (DeviceInfo.Platform != DevicePlatform.Android)
            { 
                isHandlingCheckChange = false;
                return;
            }

            bool openSettings = await DisplayAlert(
                    "Notifikace zakázány",
                    "Pro správné fungování aplikace prosím povolte notifikace v nastavení systému.",
                    "Otevřít nastavení",
                    "Zrušit");

            if (openSettings)
            {
                AppInfo.ShowSettingsUI();
            }
            else
            {
                CheckBoxNotificationCheck.IsChecked = false;
            }
            isHandlingCheckChange = false;
        }
    }

}
