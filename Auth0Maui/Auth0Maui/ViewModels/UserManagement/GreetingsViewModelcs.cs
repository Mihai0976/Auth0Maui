

using Auth0Maui.AuthO;
using Auth0Maui.ViewModels.Base;
using Microsoft.Extensions.Logging;
using System.Windows.Input;

namespace Auth0Maui.ViewModels.UserManagement;

public class GreetingsViewModel : BaseViewModel
{
    private readonly Auth0Client _auth0Client;
    private readonly ILogger<GreetingsViewModel> _logger;
    //private readonly TabVisibilityService _tabVisibilityService;
    private readonly UserSessionService _userSessionService;

    private bool _isBusy;

    public GreetingsViewModel(Auth0Client auth0Client,
        ILogger<GreetingsViewModel> logger,
        //TabVisibilityService tabVisibilityService,
        UserSessionService userSessionService)
    {
        _auth0Client = auth0Client;
        _logger = logger;

        StartAuthenticationCommand = new Command(OnStartAuthenticationClicked);
        OpenTermsAndConditionsCommand = new Command(OpenTermsAndConditions);
       // _tabVisibilityService = tabVisibilityService;
        _userSessionService = userSessionService;
    }

    public ICommand StartAuthenticationCommand { get; }
    public ICommand OpenTermsAndConditionsCommand { get; }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged(nameof(IsBusy));
        }
    }


    private async void OnStartAuthenticationClicked()
    {
        try
        {
            IsBusy = true;
            var loginResult = await _auth0Client.LoginAsync();

            if (loginResult != null && string.IsNullOrEmpty(loginResult.Error))
            {
                //Debug purpose
                //Call backend manually
                //CallBackendManually(loginResult);

                var shell = (AppShell)Shell.Current;
               // _tabVisibilityService.ShowTabs(shell, true);

                // Navigate to LoadingPage immediately after successful login
                await Shell.Current.GoToAsync("//LoadingPage");


                await _userSessionService.ValidateAndInitializeSessionAsync();

                // Introduce a delay; you can adjust the duration as needed
                await Task.Delay(500);

                // Now, navigate to homeTab
                await Shell.Current.GoToAsync("//homeTab");

                IsBusy = false;
                //await _accountViewModel.InitializeAsync();
                await DisplayError("Login Success Oauth0", "You have successfully logged in.");
                _logger.LogInformation("Authentication successful.");
            }
            else
            {
                _logger.LogError("Authentication error: {Error}", loginResult?.Error);
                await DisplayError("Authentication Error Oauth0", loginResult?.Error);
            }

        }
        catch (Exception ex)
        {
            IsBusy = false;
            _logger.LogError(ex, "Exception during authentication.");
            await DisplayError("Error", "OnStartAuthenticationClicked:" + ex.Message);
        }
    }

    private async void OpenTermsAndConditions()
    {
        try
        {
            await Launcher.OpenAsync(new Uri("https://sustenobil.ro"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception opening Terms and Conditions URL.");
            await DisplayError("Error", "Failed to open the website. Please check your connection and try again.");
        }
    }

    private async Task DisplayError(string title, string message)
    {
        if (Application.Current.MainPage != null)
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }

}
