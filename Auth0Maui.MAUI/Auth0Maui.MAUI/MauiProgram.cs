using Auth0Maui.MAUI.Auth0;
using Microsoft.Extensions.Logging;

namespace Auth0Maui.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
				fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
				fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddSingleton<AccountViewModel>();

		builder.Services.AddSingleton<AccountPage>();

		builder.Services.AddSingleton<LocationViewModel>();

		builder.Services.AddSingleton<LocationPage>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<MainPage>();
        // Set RedirectUri based on the platform
        string redirectUri;
#if WINDOWS
            redirectUri = "http://localhost/callback";
#else
        redirectUri = "myapp://callback";
#endif

        builder.Services.AddSingleton(new Auth0Client(new()
        {
            Domain = "dev-kzubp0khpigu3gf3.eu.auth0.com",
            ClientId = "ePkiUlTnyXDTVFp4mgqJnKNFguVEQ2mA",
            Scope = "openid profile",
            Audience = "https://demo-api.com",
            RedirectUri = redirectUri,
        }));

        builder.Services.AddSingleton<TokenHandler>();
        builder.Services.AddHttpClient("DemoAPI",
                client => client.BaseAddress = new Uri("http://10.0.2.2:6061")
            ).AddHttpMessageHandler<TokenHandler>();
        builder.Services.AddTransient(
            sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DemoAPI")
        );

        return builder.Build();
	}
}
