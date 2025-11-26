namespace WeatherApp;
using WeatherApp.Model;

public partial class AlertDetailPage : ContentPage
{
	public AlertDetailPage()
	{
		InitializeComponent();
	}

    public AlertDetailPage(AlertRecord alert)
    {
        InitializeComponent();
        BindingContext = alert;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        this.Opacity = 0;
        await this.FadeTo(1, 750, Easing.CubicInOut);
    }
}