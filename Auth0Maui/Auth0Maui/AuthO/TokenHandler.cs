
using System.Net.Http.Headers;
using System.Net;

namespace Auth0Maui.AuthO;


    public class TokenHandler : DelegatingHandler
    {
        private readonly Auth0Client _auth0Client;

        public TokenHandler(Auth0Client auth0Client)
        {
            _auth0Client = auth0Client;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var accessToken = await SecureStorage.Default.GetAsync("access_token");
            var refreshToken = await SecureStorage.Default.GetAsync("refresh_token");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var responseMessage = await base.SendAsync(request, cancellationToken);

            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized
                && refreshToken != null)
            {
                var refreshResult = await _auth0Client.RefreshTokenAsync(refreshToken);

                if (!refreshResult.IsError)
                {
                    await SecureStorage.Default.SetAsync("access_token", refreshResult.AccessToken);

                    request.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", refreshResult.AccessToken);

                    responseMessage = await base.SendAsync(request, cancellationToken);
                }
            }

            return responseMessage;
        }
    }
