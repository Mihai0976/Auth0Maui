using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0Maui.Services;
public class ApiExternServices
{
    private readonly HttpClient _httpClient;

    public ApiExternServices()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetCountryDataAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(
                "https://gist.githubusercontent.com/kcak11/4a2f22fb8422342b3b3daa7a1965f4e4/raw/2cc0fcb49258c667f1bc387cfebdfd3a00c4a3d5/countries.json");
            return response;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
            return null;
        }
    }
}
