using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using WeatherApp.Services;
#if ANDROID
using Plugin.Firebase.Core.Platforms.Android;
#endif
using UraniumUI;

namespace WeatherApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.RegisterFirebaseServices()
			.UseUraniumUI()
			.UseUraniumUIMaterial()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<NotificationService>();

        return builder.Build();
	}

	private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
	{

        builder.ConfigureLifecycleEvents(events => {
#if ANDROID
			events.AddAndroid(android => android.OnCreate((activity, _) =>
			CrossFirebase.Initialize(activity)));
#endif
        });

        return builder;
	}
}
