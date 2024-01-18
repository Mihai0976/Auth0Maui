namespace Auth0Maui.Domain.Models.Results
{
    public class UserLoginResult
    {
        // If login was successful, this will be true.
        // Otherwise, it will be false.
        public bool Issuccessful {  get; set; }

        // If login was successful, this will contain the JWT.
        // Otherwise, it will be null.
        public string JwtToken { get; set; }

        // The expiry date of the JWT token.
        public string ExpiryDate { get; set; }

        // If login was unsuccessful, this will contain the reason for failure.
        // Otherwise, it will be null.
        public string ErrorMessaget { get; set; }
    }
}
