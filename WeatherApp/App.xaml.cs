namespace WeatherApp
{
    public partial class App : Application
    {
        public static event EventHandler AppResumed;
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnResume()
        {
            base.OnResume();
            AppResumed?.Invoke(this, EventArgs.Empty);
        }
    }
}