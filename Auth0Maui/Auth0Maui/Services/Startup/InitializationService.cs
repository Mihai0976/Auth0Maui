

using Microsoft.Extensions.Logging;

namespace Auth0Maui.Services;

public class InitializationService
{
    private readonly ILogger<InitializationService> _logger;
    private readonly INavigationService _navigationService;
    private readonly UserSessionService _userSessionService;

    public InitializationService(INavigationService navigationService, ILogger<InitializationService> logger,
         UserSessionService userSessionService)
    {
        _navigationService = navigationService;
        _logger = logger;
        _userSessionService = userSessionService;
    }

    public async Task InitializeAppAsync()
    {
        try
        {
            var shell = (AppShell)Shell.Current;
            var isAuthenticated = await _userSessionService.ValidateAndInitializeSessionAsync();

            if (isAuthenticated)
            {
                await _navigationService.NavigateToAsync("//homeTab");
            }
            else
            {
                
                await _navigationService.NavigateToAsync("//GreetingsPage");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during app initialization: {ErrorMessage}", ex.Message);
            await DisplayAlert("Initialization Error", ex.Message);
        }
    }

    private async Task DisplayAlert(string title, string message)
    {
        if (Application.Current != null)
            if (Application.Current.MainPage != null)
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }
}
