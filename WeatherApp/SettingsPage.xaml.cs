using WeatherApp.Services;

namespace WeatherApp;

public partial class SettingsPage : ContentPage
{
    private readonly NotificationService _notificationService;

    public SettingsPage()
    {
        InitializeComponent();

        BindingContext = new SettingsViewModel();
    }

    public SettingsPage(NotificationService notificationService)
	{
		InitializeComponent();

        BindingContext = new SettingsViewModel();
    
		_notificationService = notificationService;
    }

	

    private async void PageLoad()
	{
		if (await _notificationService.IsNotificationEnabledAsync())
		{
			NotificationImage.Source = "notification_bell_enabled.png";
        }
		else
		{
            NotificationImage.Source = "notification_bell_disabled.png";
        }
    }

	private async Task<bool> IsNotificationEnable()
	{
		if (DeviceInfo.Platform != DevicePlatform.Android) return false;
        var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
		if (status != PermissionStatus.Granted)
		{
			return true;
		}
        return false;
	}

    private void CheckChange_Notification(object sender, EventArgs e)
    {

    }
}