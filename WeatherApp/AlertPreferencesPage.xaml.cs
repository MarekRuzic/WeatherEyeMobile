using WeatherApp.ViewModel;

namespace WeatherApp;

public partial class AlertPreferencesPage : ContentPage
{
	public AlertPreferencesPage()
	{
		InitializeComponent();
		BindingContext = new PreferencesViewModel();
	}

    private void AddPreferenceClicked(object sender, EventArgs e)
    {

    }
}