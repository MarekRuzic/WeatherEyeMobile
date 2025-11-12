using Plugin.Firebase.CloudMessaging;
using System.Text.Json;

namespace WeatherApp;

public partial class FirstTimeLoginPage : ContentPage
{
	public FirstTimeLoginPage()
	{
		InitializeComponent();
		PageLoad();
	}

    private bool isHandlingCheckChange = false;

    protected async void PageLoad()
    {
        await DisplayAlert("Notifikace", "Pro správné fugování a upozornění ohledně počasí je potřeba mít povolé notifikace.\n\n" +
            "Ty lze ručně povolit v nastavení aplikace nebo pomocí dilogového okénka, které se vám zobrazí.\n\n" +
            "Pomocí zaškrtávacího políčka budete informováni o stavu, zda máte notifikace zaponuté.", "Ok");



        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            }

            isHandlingCheckChange = true;
            if (status == PermissionStatus.Granted)
            {
                CheckBoxNotificationCheck.IsChecked = true;
                await DisplayAlert("Povolení", "Notifikace byly povoleny ✅", "OK");
            }
            else
            {
                CheckBoxNotificationCheck.IsChecked = false;
                await DisplayAlert("Povolení", "Notifikace nebyly povoleny ❌", "OK");
            }
            isHandlingCheckChange = false;
        }    
    }

    private async void CheckChange_Notification(object sender, EventArgs e)
    {
        if (isHandlingCheckChange) return;

        bool openSettings = await DisplayAlert(
                    "Notifikace zakázány",
                    "Pro správné fungování aplikace prosím povolte notifikace v nastavení systému.",
                    "Otevřít nastavení",
                    "Zrušit");

        if (openSettings)
        {
            AppInfo.ShowSettingsUI();
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
            if (status == PermissionStatus.Granted) CheckBoxNotificationCheck.IsChecked = true;
            else CheckBoxNotificationCheck.IsChecked = false;
        }
        else
        {
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
            if (status == PermissionStatus.Granted) CheckBoxNotificationCheck.IsChecked = true;
            else CheckBoxNotificationCheck.IsChecked = false;
        }
    }

    private async void GetFirebaseToken()
    {
        await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        Console.WriteLine($"FCM token: {token}");  
        
        SaveTokenToJSON(token);
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
        GetFirebaseToken();

        App.Current.MainPage = new NavigationPage(new MainPage());
    }
}