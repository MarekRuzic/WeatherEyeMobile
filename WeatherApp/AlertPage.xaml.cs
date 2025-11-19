namespace WeatherApp;
using WeatherApp.ViewModel;

public partial class AlertPage : ContentPage
{
	public AlertPage()
	{
		InitializeComponent();
        BindingContext = new AlertViewModel();
    }
}