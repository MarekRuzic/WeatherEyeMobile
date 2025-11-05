using WeatherApp.Services;
using WeatherApp.Model;
using System.Net.Http.Json;

namespace WeatherApp;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage()
	{
		InitializeComponent();
	}

    private async void showDialog(string title, string message, string buttons)
    {
        await App.Current.MainPage.DisplayAlert(title, message, buttons);
    }

    private async void OnClickRegistrationUser(object sender, EventArgs e)
    {
        if (this.email.Text == "" || this.email.Text == null)
        {
            showDialog("Oznámení", "Nebyl zadán email", "Ok");
            return;
        }

        if (this.password.Text == "" || this.password.Text == null)
        {
            showDialog("Oznámení", "Nebylo zadáno heslo", "Ok");
            return;
        }

        string email = this.email.Text.Trim();
        string password = this.password.Text.Trim();

        Api api = new Api();

        if (api.checkConnectivity())
        {
            showDialog("Oznámení", "Není pøipojení k internetu", "Ok");
            return;
        }

        string firstname = this.firstname.Text.Trim();
        string lastname = this.lastname.Text.Trim();

        User user = new User(firstname, lastname, email, password);

        try
        {
            HttpResponseMessage response = await api.Client.PostAsJsonAsync("user/create", user);

            if (response.IsSuccessStatusCode) showDialog("Oznámení", "Mùžete se pøihlásit", "Ok");
            else showDialog("Oznámení", "Nastala chyba", "Ok");
        }
        catch (HttpRequestException ex)
        {
            showDialog("Oznámení", "Nastala chyba", "Ok");
            showDialog("Chyba", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            showDialog("Oznámení", ex.Message, "Ok");
        }
    }
}