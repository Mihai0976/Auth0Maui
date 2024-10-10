using System.Net.Http.Headers;

namespace Auth0Maui.MAUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient; // HttpClient is injected
            _httpClient.BaseAddress = new Uri("http://10.0.2.2:5226");
        }

        public async Task<string> GetUsers(string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var response = await _httpClient.GetAsync("/api/authentification"); // Assume base URL is set in MauiProgram

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }



    }
}

