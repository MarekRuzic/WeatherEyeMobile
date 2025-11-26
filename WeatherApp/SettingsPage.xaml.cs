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
            "Uložete prosím znovu vaše zvolené nastavení v tomto menu pomocí tlačítka dole na stránce ⬇️\n\n" +
            "Poté by mělo vše fungovat 😁", "Ok");

        // Toto už jde mimo uživatele - pouze pro testování

        await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        Console.WriteLine($"FCM token: {token}");
        bool result = await DisplayAlert("FCM Token", token + "\n\n\nPřeje si daný token zkopírovat do schánky?", "Ano", "Ne");
        if (result)
        {
            await Clipboard.Default.SetTextAsync(token);
        }
    }

    private void SaveSettingsClicked(object sender, EventArgs e)
    {
        string region = ((SettingsViewModel)BindingContext).SelectedRegion;
        string eventType = ((SettingsViewModel)BindingContext).SelectedEventType;
        string threatLevel = ((SettingsViewModel)BindingContext).SelectedThreatLevel;
        bool appNotificationsEnabled = AppNotificationCheck.IsChecked;
        bool emailEnabled = EmailNotificationCheck.IsChecked;

        DisplayAlert("Oznámení", "Region: " + region + "\n" +
            "Event: " + eventType + "\n" +
            "Nebezpečí: " + threatLevel + "\n" +
            "App notifikace: " + (appNotificationsEnabled ? "Povolené" : "Zakázané") + "\n" +
            "Email notifkace: " + (emailEnabled ? "Povolené" : "Zakázané"), "Ok");
    }


    //Momentálně nikde nepoužívám
    private async Task<string> LoadTokenFromJSON()
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "token.json");

        if (!File.Exists(filePath))
        {
            await DisplayAlert("Chyba", "Soubor s tokenem neexistuje.", "OK");
            return null;
        }

        string json = await File.ReadAllTextAsync(filePath);
        var tokenData = JsonSerializer.Deserialize<FirebaseToken>(json);

        await DisplayAlert("Token načten", $"Token: {tokenData.token}", "OK");
        return tokenData.token;
    }
}