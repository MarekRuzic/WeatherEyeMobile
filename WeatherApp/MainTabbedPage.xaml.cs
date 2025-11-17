namespace WeatherApp;

public partial class MainTabbedPage : TabbedPage
{
    public MainTabbedPage()
    {
        InitializeComponent();
    }

    /*public MainTabbedPage(
        MainPage mainPage,
        AlertPage alertPage,
        SettingsPage settingsPage)
    {
        InitializeComponent();

        // pøidání stránek
        Children.Add(mainPage);
        Children.Add(alertPage);
        Children.Add(settingsPage);

        // ikony + názvy
        mainPage.Title = "Domov";
        mainPage.IconImageSource = "home_icon.png";

        alertPage.Title = "Upozornìní";
        alertPage.IconImageSource = "alert_icon.png";

        settingsPage.Title = "Nastavení";
        settingsPage.IconImageSource = "settings_icon.png";
    }*/
}