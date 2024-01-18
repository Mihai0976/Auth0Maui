
namespace Auth0Maui.Domain.Models.Commands.UserManagement.Auth
{
    public class TokenExchangeExternalCommand
    {
        public string AccessToken { get; set; }
        public string IdentityToken { get; set; }
        public string RefresherToken { get; set; }
        public DateTimeOffset ExpirationAccessToken { get; set; }
    }
}
