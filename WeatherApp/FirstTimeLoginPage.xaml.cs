using Plugin.Firebase.CloudMessaging;
using System.Text.Json;
using WeatherApp.Model;
using WeatherApp.Services;

namespace WeatherApp;

public partial class FirstTimeLoginPage : ContentPage
{
    private readonly UserApiService _userApiService = new UserApiService();
    private readonly NotificationService _notificationService = new();
    private readonly bool _openFromSettings;

    public FirstTimeLoginPage()
	{
		InitializeComponent();
		PageLoad();
        App.AppResumed += (s, e) => CheckNotification();
    }

    public FirstTimeLoginPage(bool openFromSettings)
    {
        InitializeComponent();
        this._openFromSettings = openFromSettings;
        if (openFromSettings)
        {
            SaveMobileTokenButton.Text = "Uložit pro opravení fungování notifikací";
            CheckNotification();
        }
        else
        {
            PageLoad();
        }
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
        isHandlingCheckChange = true;
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

    private async void OnClickGoToMainPage(object sender, EventArgs e)
    {
        SaveMobileTokenButton.IsVisible = false;
        LoadingIndicator.IsVisible = true;
        try
        {
            string token = await GetFirebaseToken();
            //string token = "Muj_testToken";
            bool result = await _userApiService.SaveMobAppIdAsync(token);
            if (!result)
            {
                result = await DisplayAlert("Problém s uložením údajů", "Při ukládání údajů k notifikacím došlo k chybě.\n\n" +
                    "Proto Vám zřejmě nebudou notifikace chodit.\n\n" +
                    "Doporučujeme si uložení opakovat!\n\n" +
                    "Případně můžete znovu tuto akci provést v nastavení.\n\n" +
                    "Přejete si opakovat uložení?", "Ano", "Ne");

                if (result)
                {
                    SaveMobileTokenButton.IsVisible = true;
                    LoadingIndicator.IsVisible = false;
                    return;
                }
            }

            if (_openFromSettings)
            {
                await Navigation.PopAsync();
                return;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Chyba", "Při ukládání údajů k notifikacím došlo k chybě.", "OK");
        }
        App.Current.MainPage = new NavigationPage(new MainTabbedPage());
    }    
}