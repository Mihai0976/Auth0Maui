using IdentityModel.Client;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient;
using System.Text;
using IdentityModel.OidcClient.Results;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auth0Maui.Domain.Models.DTOs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Auth0Maui.AuthO
{
    public class Auth0Client
    {
        private readonly string _audience;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly OidcClient _oidcClient;
        private readonly Auth0ClientOptions _options;

        public Auth0Client(Auth0ClientOptions options, HttpClient httpClient,
            ILogger logger)
        {
            _options = options;
            _httpClient = httpClient;
            _oidcClient = new OidcClient(new OidcClientOptions
            {
                Authority = $"https://{options.Domain}",
                ClientId = options.ClientId,
                Scope = options.Scope,
                RedirectUri = options.RedirectUri,
                Browser = (IdentityModel.OidcClient.Browser.IBrowser)options.Browser
            });

            _audience = options.Audience;
            _logger = logger;
        }

        public async Task<UserLoginModel> LoginAsync()
        {
            try
            {
                var loginRequest = PrepareLoginRequest();
                var loginResult = await _oidcClient.LoginAsync(loginRequest);
                if (loginResult.IsError)
                {
                    await LogAndDisplayError("Login to Oauth0 failed.", $"Login error: {loginResult.Error}");
                    return null;
                }


                // Prepare to exchange token with backend
                var apiService = new ApiService();
                var oauth0Info = new TokenExchangeDto
                {
                    AccessToken = loginResult.AccessToken,
                    IdentityToken = loginResult.IdentityToken,
                    RefreshToken = loginResult.RefreshToken,
                    AccessTokenExpiration = loginResult.AccessTokenExpiration
                };

                var exchangeTokenResponse = await apiService.ExchangeToken(oauth0Info);
                if (exchangeTokenResponse == null)
                {
                    await LogAndDisplayError($"Excepton during exchange token");
                    return null;
                }
                var exchangeTokenJson = JObject.Parse(exchangeTokenResponse);
                var token = exchangeTokenJson["token"]?.ToString();

                //for the moment save the one from 0Auth, later we will save ours
                await SaveTokensAsync(token);

                var userClaims = DecodeToken(token);
                if (userClaims == null) return null;

                var model = CreateUserLoginModel(userClaims);
                if (token != null) model.IdentityToken = model.AccessToken = token;

                return model;
            }
            catch (Exception e)
            {
                await LogAndDisplayError($"Exception during LoginAsync: {e.Message}", e.ToString());
                return null;
            }
        }

        public async Task<BrowserResult> LogoutAsync()
        {
            var logoutParameters = new Dictionary<string, string>
        {
            { "client_id", _oidcClient.Options.ClientId },
            { "returnTo", _oidcClient.Options.RedirectUri }
        };

            var logoutRequest = new LogoutRequest();
            var endSessionUrl = new RequestUrl($"{_oidcClient.Options.Authority}/v2/logout")
                .Create(new Parameters(logoutParameters));

            var browserOptions = new BrowserOptions(endSessionUrl, _oidcClient.Options.RedirectUri)
            {
                Timeout = TimeSpan.FromSeconds(logoutRequest.BrowserTimeout),
                DisplayMode = logoutRequest.BrowserDisplayMode
            };

            var browserResult = await _oidcClient.Options.Browser.InvokeAsync(browserOptions);
            SecureStorage.Default.RemoveAll();
            return browserResult;
        }

        public async Task<RefreshTokenResult> RefreshTokenAsync(string refreshToken)
        {
            return await _oidcClient.RefreshTokenAsync(refreshToken);
        }

        public async Task<bool> SendEmailPasswordChange(string email)
        {
            var requestUrl = $"https://{_options.Domain}/dbconnections/change_password";
            var requestData = new
            {
                client_id = _options.ClientId,
                email,
                connection = "Username-Password-Authentication"
            };

            var requestContent =
                new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(requestUrl, requestContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendEmailPasswordChange: {ex.Message}");
                return false;
            }
        }


        private UserLoginModel CreateUserLoginModel(IEnumerable<Claim> claims)
        {
            var model = new UserLoginModel();
            model.PopulateFromClaims(claims.ToList());

            return model;
        }

        //private async Task<UserLoginModel> HandleTokenExpirationAsync(string refreshToken)
        //{
        //    //var refreshResult = await RefreshTokenAsync(refreshToken);
        //    //if (!refreshResult.IsError)
        //    //{
        //    //    await SaveTokensAsync(refreshResult);
        //    //    return await GetAuthenticatedUserAsync(); // Retrieve user with new token
        //    //}

        //    //await _navigationService.NavigateToAsync("//LoginPage");
        //    return null;
        //}

        //
        //private async Task<TokenValidationResult> ValidateTokenAsync(string token)
        //{
        //    var doc = await httpClient.GetDiscoveryDocumentAsync(oidcClient.Options.Authority);
        //    if (doc.IsError) throw new InvalidOperationException("Discovery document retrieval failed.");

        //    var securityKeys = doc.KeySet.Keys.Select(CreateSecurityKeyFromJsonWebKey).ToList();

        //    var options = new TokenValidationParameters
        //    {
        //        ValidIssuer = doc.Issuer,
        //        ValidAudience = oidcClient.Options.ClientId,
        //        IssuerSigningKeys = securityKeys
        //    };

        //    var handler = new JwtSecurityTokenHandler();
        //    try
        //    {
        //        var principal = handler.ValidateToken(token, options, out var validatedToken);
        //        return new TokenValidationResult
        //        {
        //            ClaimsIdentity = principal.Identity as ClaimsIdentity,
        //            SecurityToken = validatedToken
        //        };
        //    }
        //    catch (SecurityTokenExpiredException ex)
        //    {
        //        return new TokenValidationResult { Exception = ex };
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle other exceptions as needed
        //        return new TokenValidationResult { Exception = ex };
        //    }
        //}

        //private async Task<UserLoginModel> HandleTokenExpirationAsync()
        //{
        //    var refreshToken = await SecureStorage.Default.GetAsync("refresh_token");
        //    if (refreshToken == null) return null;

        //    var refreshResult = await RefreshTokenAsync(refreshToken);
        //    return !refreshResult.IsError ? await GetAuthenticatedUserAsync() : null;
        //}

        private LoginRequest PrepareLoginRequest()
        {
            return new LoginRequest
            {
                FrontChannelExtraParameters = !string.IsNullOrEmpty(_audience)
                    ? new Parameters(new Dictionary<string, string> { { "audience", _audience } })
                    : null
            };
        }

        private async Task SaveTokensAsync(string refeeltoken)
        {
            //await SecureStorage.Default.SetAsync("access_token", result.AccessToken);
            await SecureStorage.Default.SetAsync("access_token", refeeltoken);

            //from 0auth for the moment
            await SecureStorage.Default.SetAsync("id_token", refeeltoken);

            //var accessTokenExpiration = result.AccessTokenExpiration.DateTime;
            //await SecureStorage.Default.SetAsync("access_token_expiration", accessTokenExpiration.ToString());

            //if (!string.IsNullOrEmpty(result.RefreshToken))
            //{
            //    await SecureStorage.Default.SetAsync("refresh_token", result.RefreshToken);
            //}
        }

        private IEnumerable<Claim> DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims;
        }

        private async Task LogAndDisplayError(string message, string logMessage = null)
        {
            _logger.LogError(logMessage ?? message);
            if (Application.Current != null)
                if (Application.Current.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", message, "OK");
        }
    }
}
