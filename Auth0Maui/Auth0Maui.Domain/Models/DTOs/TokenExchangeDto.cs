

namespace Auth0Maui.Domain.Models.DTOs
{
    public class TokenExchangeDto
    {
        public string AccessToken { get; set; }
        public string IdentityToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTimeOffset AccessTokenExpiration { get; set; }
    }
}
