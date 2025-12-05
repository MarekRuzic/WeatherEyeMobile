namespace WeatherApp;

using System.Threading.Tasks;
using WeatherApp.ViewModel;

public partial class AlertPage : ContentPage
{
	private readonly AlertViewModel _viewModel;

    public AlertPage()
	{
		InitializeComponent();
		_viewModel = new AlertViewModel();
        BindingContext = _viewModel;
    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		
		if (!_viewModel.IsInitialized)
		{
			await _viewModel.InitializeAsync();
        }
    }
}