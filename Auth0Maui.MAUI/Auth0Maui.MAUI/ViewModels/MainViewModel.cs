using Auth0Maui.Domain.Models.DTOs;
using Auth0Maui.MAUI.Auth0;
using Auth0Maui.MAUI.Models;
using System.Windows.Input;

namespace Auth0Maui.MAUI.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private AccountViewModel _accountViewModel;
    private string _username;
    private ImageSource _userPicture;
    private bool _isHomeViewVisible;
    private bool _isLoginViewVisible;
    private bool _isLoginButtonVisible = true;
    



    // Use Auth0Client and HttpClient as needed
    private readonly Auth0Client _auth0Client;
    private readonly HttpClient _httpClient;


    public bool IsLoginButtonVisible
    {
        get => _isLoginButtonVisible;
        set => SetProperty(ref _isLoginButtonVisible, value);
    }

    // Username property
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    // UserPicture property
    public ImageSource UserPicture
    {
        get => _userPicture;
        set => SetProperty(ref _userPicture, value);
    }

    // IsHomeViewVisible property
    public bool IsHomeViewVisible
    {
        get => _isHomeViewVisible;
        set => SetProperty(ref _isHomeViewVisible, value);
    }

    // IsLoginViewVisible property
    public bool IsLoginViewVisible
    {
        get => _isLoginViewVisible;
        set => SetProperty(ref _isLoginViewVisible, value);
    }

    // Commands
    public ICommand LoginCommand { get; }
    public ICommand LogoutCommand { get; }


    public MainViewModel(Auth0Client auth0Client, HttpClient httpClient, AccountViewModel accountViewModel)
    {
        _auth0Client = auth0Client;
        _httpClient = httpClient;
        _accountViewModel = accountViewModel;
        LoginCommand = new Command(async () => await ExecuteLoginCommand());
        LogoutCommand = new Command(async () => await ExecuteLogoutCommand());
        IsHomeViewVisible = false;
    }


    private async Task ExecuteLogoutCommand()
    {
        // Perform logout logic...
        IsLoginViewVisible = true;
        IsHomeViewVisible = false;
        // Clear user information and token
        Username = string.Empty;
        UserPicture = null;
        TokenHolder.AccessToken = null;
        IsLoginButtonVisible = true;
    }


    private async Task ExecuteLoginCommand()
    {
        var loginResult = await _auth0Client.LoginAsync();

        if (!loginResult.IsError)
        {
          
            var userDto = new UserDto
            {
             
                Email = loginResult.User.FindFirst(c => c.Type == "email")?.Value,
                Name = loginResult.User.FindFirst(c => c.Type == "name")?.Value,
                FamilyName = loginResult.User.FindFirst(c => c.Type == "family_name")?.Value,
                PictureUrl = loginResult.User.FindFirst(c => c.Type == "picture")?.Value
            };
            // Update ViewModel properties instead of directly setting UI elements
            Username = loginResult.User.Identity.Name;
            UserPicture = ImageSource.FromUri(new Uri(loginResult.User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value));

            // Update visibility of views
            IsLoginViewVisible = false;
            IsHomeViewVisible = true;
            IsLoginButtonVisible = false;

            _accountViewModel.InitializeWithUserData(userDto);

            // Store access token
            TokenHolder.AccessToken = loginResult.AccessToken;
        }
        else
        {
            // Use a messaging center or a service to show the alert
            MessagingCenter.Send(this, "DisplayAlert", new AlertMessage { Title = "Error", Message = loginResult.ErrorDescription, Cancel = "OK" });
        }
    }

}
