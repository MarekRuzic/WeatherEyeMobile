using WeatherApp.ModelAuth;
using WeatherApp.Services;

namespace WeatherApp;

public partial class LoginPage : ContentPage
{
    private readonly LoginService _loginService = new LoginService();

	public LoginPage()
	{
		InitializeComponent();
	}

    private void ShowLoadingIndicator(bool show)
    {
        if (show)
        {
            LoginButton.IsVisible = false;
            LoadingIndicator.IsVisible = true;
            return;
        }
        LoginButton.IsVisible = true;
        LoadingIndicator.IsVisible = false;
    }

    private async void OnClickLogin(object sender, EventArgs e)
    {
        string username = Email.Text.Trim();
        string password = Password.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Chyba", "Nebyly vyplněny přihlašovací údaje 😭", "Ok");
            return;
        }

        ShowLoadingIndicator(true);

        AuthResponse authResponseResult = await _loginService.LoginAsync(username, password);

        // Timeout detekce
        if (authResponseResult?.access_token == "TIMEOUT")
        {
            ShowLoadingIndicator(false);
            await DisplayAlert("Chyba", "Server neodpovídá. Zkuste to prosím znovu. ⏱️", "OK");
            return;
        }

        if (authResponseResult == null)
        {
            await DisplayAlert("Chyba", "Nesprávné jméno/email nebo heslo 😅", "Ok");
            ShowLoadingIndicator(false);
            return;
        }

        // Uložení věcí do SecureStorage
        await AuthStorage.SaveTokensAsync(authResponseResult, username);


		bool isLoggedInFirstTime = Preferences.Get("IsLoggedInFirstTime", true);
        // Pokud se uživatel přihlašuje poprvné bude přesměrován na stránku s povolením notifikací
        ShowLoadingIndicator(false);
        if (isLoggedInFirstTime)
		{
            Preferences.Set("IsLoggedInFirstTime", false);
            App.Current.MainPage = new NavigationPage(new FirstTimeLoginPage());
            return;
        }
        App.Current.MainPage = new NavigationPage(new MainTabbedPage());        
    }
}