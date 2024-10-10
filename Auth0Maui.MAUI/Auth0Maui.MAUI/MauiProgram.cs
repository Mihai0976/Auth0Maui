using Auth0Maui.MAUI.Auth0;
using Auth0Maui.MAUI.Services;
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
        builder.Services.AddSingleton<ApiService>();

        // Set the BaseAddress based on the platform
        string baseUrl;
#if ANDROID
        baseUrl = "http://10.0.2.2:5226"; // Android emulator accessing localhost
#else
        baseUrl = "http://localhost:5226"; // Desktop or other platforms
#endif

        builder.Services.AddHttpClient("DemoAPI", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Only for dev
        })
        .AddHttpMessageHandler<TokenHandler>();

        builder.Services.AddTransient(
            sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DemoAPI")
        );

        return builder.Build();
    }
}
