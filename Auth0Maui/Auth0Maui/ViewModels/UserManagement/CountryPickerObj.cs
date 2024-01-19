using Newtonsoft.Json;


namespace Auth0Maui.ViewModels.UserManagement;

public class CountryPickerObj
{
    [JsonProperty("name")] public string CountryName { get; set; }

    [JsonProperty("flag")] public string CountryFlag { get; set; }

    [JsonProperty("dialCode")] public string CountryTelCode { get; set; }

    [JsonProperty("isoCode")] public string CountryIsoCode { get; set; }
}