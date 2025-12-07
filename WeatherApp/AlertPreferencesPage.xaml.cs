using WeatherApp.ViewModel;

namespace WeatherApp;

public partial class AlertPreferencesPage : ContentPage
{
	public AlertPreferencesPage()
	{
		InitializeComponent();
		BindingContext = new PreferencesViewModel();
	}

    private async void AddPreferenceClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Preference", "Mobilní aplikace neumí měnit ani přidat nové prefence je potřeba " +
                                                       "přejít na webovou stránku aplikace.\n\n" +
            "Přejete si pokračovat?", "Ano", "Ne");

        if (!result) return;

        try
        {
            Uri uri = new Uri("https://weathereye.eu/notifications");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }
}