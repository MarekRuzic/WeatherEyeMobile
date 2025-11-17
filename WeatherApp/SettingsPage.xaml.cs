using WeatherApp.Services;

namespace WeatherApp;

public partial class SettingsPage : ContentPage
{
    private readonly NotificationService _notificationService = new NotificationService();
    private bool isHandlingCheckChange = false;

    public SettingsPage()
    {
        InitializeComponent();

        BindingContext = new SettingsViewModel();
        PageLoad();
        App.AppResumed += (s, e) => PageLoad();
    }
	

    private async void PageLoad()
	{
        isHandlingCheckChange = true;
		if (await _notificationService.IsNotificationEnabledAsync())
		{
			NotificationImage.Source = "notification_bell_enabled.png";
            CheckBoxNotificationCheck.IsChecked = true;
            CheckBoxNotificationCheck.IsEnabled = false;
        }
		else
		{
            NotificationImage.Source = "notification_bell_disabled.png";
            CheckBoxNotificationCheck.IsChecked = false;
            CheckBoxNotificationCheck.IsEnabled = true;
        }
        isHandlingCheckChange = false;
    }

    
    private async void CheckChange_Notification(object sender, EventArgs e)
    {
        if (isHandlingCheckChange) return;

        if (DeviceInfo.Platform != DevicePlatform.Android)
        {
            isHandlingCheckChange = false;
            return;
        }

        isHandlingCheckChange = true;

        bool openSettings = await DisplayAlert(
                    "Notifikace zakázány",
                    "Pro správné fungování aplikace prosím povolte notifikace v nastavení systému.",
                    "Otevøít nastavení",
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