namespace WeatherApp;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnClickLogin(object sender, EventArgs e)
    {
		bool isLoggedInFirstTime = Preferences.Get("IsLoggedInFirstTime", true);
        // Pokud se uživatel pøihlašuje poprvné bude pøesmìrován na stránku s povolením notifikací
		if (isLoggedInFirstTime)
		{
            Preferences.Set("IsLoggedInFirstTime", false);
            App.Current.MainPage = new NavigationPage(new FirstTimeLoginPage());
            return;
        }
        App.Current.MainPage = new NavigationPage(new MainPage());	
    }
}