namespace AuthoMaui.Aplication.Models.DTOs;

public class UserRegistrationDto
{
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public DateTimeOffset? AccessTokenExpiration { get; set; }
    public string IdentityToken { get; set; }
    public string RefreshToken { get; set; }
    public string Nickname { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public string SessionId { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string Locale { get; set; }
    public bool EmailVerified { get; set; }
    public bool IsAuthenticated { get; set; }
}
