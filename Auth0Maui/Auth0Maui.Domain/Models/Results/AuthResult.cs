

/// <summary>
///     Represents the result of an authentication process.
///     This class is designed to be immutable to ensure that once the authentication result is created, it can't be
///     modified accidentally in the application.
/// </summar

namespace Auth0Maui.Domain.Models.Results
{
    public class AuthResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthResult" /> class.
        /// </summary>
        /// <param name="isAuthenticated">A value indicating whether the user is authenticated.</param>
        /// <param name="jwtToken">The JSON Web Token (JWT) that represents the authenticated session.</param>
        /// <param name="refreshToken">The refresh token that can be used to refresh the JWT when it expires.</param>
        /// <param name="expiryDate">The expiry date of the JWT.</param>
        
        public AuthResult(bool isAuthenticated, string jwtToken, string refreshToken, DateTime expiryDate)
        {
            IsAuthenticated = isAuthenticated;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;    
            ExpiryDate = expiryDate;
            
        }

        /// <summary>
        ///     Gets a value indicating whether the user is authenticated.
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        ///     Gets the JSON Web Token (JWT) that represents the authenticated session.
        /// </summary>
        public string JwtToken { get; set;}

        /// <summary>
        ///     Gets the refresh token that can be used to refresh the JWT when it expires.
        /// </summary>
        public string RefreshToken { get; set;}

        /// <summary>
        ///     Gets the expiry date of the JWT.
        /// </summary>
        public DateTime ExpiryDate { get; } 

        internal static AuthResult CreateSuccesfullResult(object accessToken, object refreshToken, object expiryDate)
        {
            throw new NotImplementedException();
        }

    }
}
