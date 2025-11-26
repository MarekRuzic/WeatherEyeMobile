using Plugin.Firebase.CloudMessaging;
using System.Text.Json;
using WeatherApp.Model;
using WeatherApp.Services;

namespace WeatherApp;

public partial class FirstTimeLoginPage : ContentPage
{
    private readonly NotificationService _notificationService = new();

    public FirstTimeLoginPage()
	{
		InitializeComponent();
		PageLoad();
        App.AppResumed += (s, e) => CheckNotification();
    }

    private bool isHandlingCheckChange = false;

    protected async void PageLoad()
    {
        await DisplayAlert("Notifikace", "Pro správné fugování a upozornění ohledně počasí je potřeba mít povolé notifikace.\n\n" +
            "Ty lze ručně povolit v nastavení aplikace nebo pomocí dilogového okénka, které se vám zobrazí.\n\n" +
            "Pomocí zaškrtávacího políčka budete informováni o stavu, zda máte notifikace zaponuté.", "Ok");


        CheckNotification();      
    }

    protected async void CheckNotification()
    {
        bool granted = await _notificationService.RequestNotificationPermissionAsync();

        if (granted)
        {
            CheckBoxNotificationCheck.IsChecked = true;
            CheckBoxNotificationCheck.IsEnabled = false;
            NotificationImage.Source = "notification_bell_enabled.png";
            Notification_Info.Text = "(Notifikace jsou povolené)";

            await DisplayAlert("Povolení", "Notifikace byly povoleny ✅", "OK");
        }
        else
        {
            CheckBoxNotificationCheck.IsChecked = false;
            await DisplayAlert("Povolení", "Notifikace nebyly povoleny ❌", "OK");
        }
        isHandlingCheckChange = false;        
    }

    private async void CheckChange_Notification(object sender, EventArgs e)
    {
        if (isHandlingCheckChange) return;

        bool openSettings = await DisplayAlert(
                    "Notifikace zakázány",
                    "Pro správné fungování aplikace prosím povolte notifikace v nastavení systému.",
                    "Otevřít nastavení",
                    "Zrušit");

        isHandlingCheckChange = true;

        if (openSettings)
        {
            AppInfo.ShowSettingsUI();            
        }
        CheckBoxNotificationCheck.IsChecked = false;

        isHandlingCheckChange = false;
    }

    private async Task<string> GetFirebaseToken()
    {
        string token = string.Empty;
        await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        Console.WriteLine($"FCM token: {token}");

        return token;        
    }

    private async void SaveTokenToJSON(string token)
    {
        FirebaseToken firebaseToken = new FirebaseToken(token);
        string json = JsonSerializer.Serialize(firebaseToken, new JsonSerializerOptions { WriteIndented = true });
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "token.json");

        await File.WriteAllTextAsync(filePath, json);

        await DisplayAlert("Hotovo", $"Token uložen do:\n{filePath}", "OK");
    }

    private async void OnClickGoToMainPage(object sender, EventArgs e)
    {
        string token = await GetFirebaseToken();
        SaveTokenToJSON(token);

        App.Current.MainPage = new NavigationPage(new MainTabbedPage());
    }    
}