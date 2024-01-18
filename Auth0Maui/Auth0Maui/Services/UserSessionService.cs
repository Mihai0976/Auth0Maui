using Auth0Maui.AuthO;
using Microsoft.Extensions.Logging;
using Auth0Maui.Domain.Models.DTOs;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Auth0Maui.Services;
public class UserSessionService
{
    private readonly ApiService _apiService = new();
    private readonly Auth0Client _auth0Client;
    private readonly ILogger<UserSessionService> _logger;

    public UserSessionService(Auth0Client auth0Client, ILogger<UserSessionService> logger)
    {
        _auth0Client = auth0Client;
        _logger = logger;
    }

    public UserLoginModel CurrentUser { get; private set; }

    public async Task<bool> ValidateAndInitializeSessionAsync()
    {
        var userClaims = await GetAuthenticatedUserAsync();
        if (userClaims != null)
        {
            await InitializeUserDetails(userClaims.UserId);
            return true;
        }

        return false;
    }

    private async Task InitializeUserDetails(string userId)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            var userDetails = await _apiService.GetUserById(userId);
            if (userDetails != null) CurrentUser = MapToUserLoginModel(userDetails);
        }
    }

    private UserLoginModel MapToUserLoginModel(UserDetailsDto userDetails)
    {
        var userLoginModel = new UserLoginModel
        {
            Id = Guid.Parse(userDetails.Id),
            Email = userDetails.Email,
            Name = userDetails.Name,
            PictureUrl = userDetails.UserPictureUrl,
            //AccessTokenExpiration = userDetails.AccessTokenExpiration,
            //AccessToken = userDetails.AccessToken,
            //Audience =   userDetails.Audience,
            //EmailVerified = userDetails.EmailVerified,
            //Error = userDetails.Error,
            //Expiration = userDetails.Expiration,
            //ExpiresIn = userDetails.ExpiresIn,
            //FamilyName = userDetails.Name,
            GivenName = userDetails.Surname
            //IdentityToken = userDetails.IdentityToken,
            //Nickname = userDetails.Nickname,   
            //IsError = userDetails.IsError,
            //IssuedAt = userDetails.IssuedAt,
            //Issuer = userDetails.Issuer,
            //Locale = userDetails.Locale,
            //RefreshToken = userDetails.RefreshToken,
            //Scope = userDetails.Scope,
            //SessionId = userDetails.SessionId,
            //TokenType = userDetails.TokenType,
            //UpdatedAt = userDetails.UpdatedAt,
            //UserId = userDetails.UserId,
        };
        return userLoginModel;
    }

    public async Task InitiatePasswordChange()
    {
        if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.Email))
        {
            var result = await _auth0Client.SendEmailPasswordChange(CurrentUser.Email);
            if (result)
            {
                // Handle success - Show success alert to user
                if (Application.Current != null)
                    if (Application.Current.MainPage != null)
                        await Application.Current.MainPage.DisplayAlert("Success",
                            "Password reset email sent successfully.", "OK");
            }
            else
            {
                // Handle failure - Show error alert to user
                if (Application.Current != null)
                    if (Application.Current.MainPage != null)
                        await Application.Current.MainPage.DisplayAlert("Error",
                            "Failed to send password reset email.", "OK");
            }
        }
    }

    public async Task<UserLoginModel> GetAuthenticatedUserAsync()
    {
        try
        {
            var idToken = await SecureStorage.GetAsync("id_token");
            if (string.IsNullOrEmpty(idToken)) return null;
            var validationResult = DecodeToken(idToken);
            if (validationResult == null)
                // Handle invalid token
                return null;
            var userModel = CreateUserLoginModel(validationResult);
            userModel.AccessToken = await SecureStorage.GetAsync("access_token") ??
                                    throw new InvalidOperationException();
            userModel.IdentityToken = idToken;
            return userModel;
        }
        catch (Exception e)
        {
            await LogAndDisplayError($"Exception during GetAuthenticatedUserAsync: {e.Message}", e.ToString());
            return null;
        }
    }

    private async Task LogAndDisplayError(string message, string logMessage = null)
    {
        _logger.LogError(logMessage ?? message);
        if (Application.Current != null)
            if (Application.Current.MainPage != null)
                await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
    }

    private UserLoginModel CreateUserLoginModel(IEnumerable<Claim> claims)
    {
        var model = new UserLoginModel();
        model.PopulateFromClaims(claims.ToList());
        return model;
    }

    private IEnumerable<Claim> DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Claims;
    }
}
