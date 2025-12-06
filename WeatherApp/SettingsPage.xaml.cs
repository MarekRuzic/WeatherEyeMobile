using Plugin.Firebase.CloudMessaging;
using System.Text.Json;
using WeatherApp.Model;
using WeatherApp.Services;
using WeatherApp.ViewModel;

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

    // Toto později smazat - zobrazuje token pro testování
    // Nebo upravit, tak aby se obnovil případně token uživateli do databáze, kdyby se náhodou změnil, případně to řešit při ukládání celého nastavení
    private async void NotificationButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Oznámení", "V případě, že máte notifikace povolení a stále Vám žádné nechodí, mohl nastast problém s připojením k databázi.\n\n" +
            "Uložete prosím znovu vaše zvolené nastavení v teď otevřeném menu pomocí tlačítka dole na stránce ⬇️\n\n" +
            "Poté by mělo vše fungovat 😁", "Ok");

        await Navigation.PushAsync(new FirstTimeLoginPage(true));        
    }    

    private async void AlertPreferenceButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlertPreferencesPage());
    }
}