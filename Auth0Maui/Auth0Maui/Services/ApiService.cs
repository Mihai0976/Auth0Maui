
using Auth0Maui.Domain.Models.DTOs;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Auth0Maui.Services;

public class ApiService
{
    private readonly HttpClient _client;

    public ApiService()
    {
        //ToDo remove is only for DEV to bypass SSL
        // Create an HttpClientHandler that ignores SSL certificate errors
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

        // Use the handler with HttpClient
        _client = new HttpClient(handler)
        {

            //good one
            BaseAddress = new Uri("http://192.168.0.180:5000/")
            //debug
            // BaseAddress = new Uri("https://192.168.0.180:32768/")

            //9229
            //5000
        };
    }


    public async Task<string> RegisterUser(UserRegistationModel user)
    {
        try
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/v2/users/register", content);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<HttpResponseMessage> UpdateUser(UpdateUserModel updateUserDto)
    {
        try
        {
            var token = await GetSavedToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Serialize the DTO to JSON
            var json = JsonConvert.SerializeObject(new { model = updateUserDto });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the PUT request
            var response = await _client.PutAsync($"api/v2/users/{updateUserDto.Id}", content);
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<UserLoginModel> GetUserByEmail(string email)
    {
        var token = await GetSavedToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var encodedEmail = WebUtility.UrlEncode(email);
        var response = await _client.GetAsync($"api/internal/users/details?email={encodedEmail}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserLoginModel>(json);
        }

        return null;
    }

    public async Task<UserDetailsDto> GetUserById(string userId)
    {
        try
        {
            var token = await GetSavedToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"api/v2/users/user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserDetailsDto>(json);
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            // Handle exception
            return null;
        }
    }

    public async Task<string> ExchangeToken(TokenExchangeDto tokenDto)
    {
        try
        {
            var token = new
            {
                accessToken = tokenDto.AccessToken,
                identityToken = tokenDto.IdentityToken,
                refreshToken = tokenDto.RefreshToken,
                accessTokenExpiration = tokenDto.AccessTokenExpiration
            };
            var json = JsonConvert.SerializeObject(token);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/v2/auth/token", content);
            if (response.IsSuccessStatusCode)
            {
                var exchangedToken = await response.Content.ReadAsStringAsync();
                return exchangedToken;
            }

            // Handle error

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<string> GetElearningModules()
    {
        return await _client.GetStringAsync("elearning");
    }

    public async Task<string> GetRethinkGames()
    {
        return await _client.GetStringAsync("rethink/games");
    }

    public async Task<string> GetRefuseGames()
    {
        return await _client.GetStringAsync("refuse/games");
    }

    public async Task<string> GetReduceGames()
    {
        return await _client.GetStringAsync("reduce/games");
    }

    public async Task<string> GetReusePosts()
    {
        return await _client.GetStringAsync("reuse/posts");
    }

    public async Task<string> GetLinesOfBusinessActions()
    {
        return await _client.GetStringAsync("business/actions");
    }

    public async Task<string> GetRepairCenters()
    {
        return await _client.GetStringAsync("repair/centers");
    }

    public async Task<string> GetRegiftOptions()
    {
        return await _client.GetStringAsync("regift/options");
    }

    public async Task<string> GetRecycleOptions()
    {
        return await _client.GetStringAsync("recycle/options");
    }

    private async Task<string> GetSavedToken()
    {
        return await SecureStorage.Default.GetAsync("access_token");
    }
}
