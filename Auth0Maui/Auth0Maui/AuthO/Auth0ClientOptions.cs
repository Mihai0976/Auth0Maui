using Auth0Maui.MAUI.Auth0;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace Auth0Maui.AuthO;

    public class Auth0ClientOptions
    {
        public Auth0ClientOptions()
        {
            //Scope = "openid";
            //Scope = "openid profile email";
            //RedirectUri = "com.auth0.quickstart://dev-z3vtx0oof4q3xmti.eu.auth0.com/android/com.auth0.quickstart/callback";
            Browser = new WebBrowserAuthenticator();
            Audience = "";
        }

        public string Domain { get; set; }

        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string Scope { get; set; }

        public string Audience { get; set; }

        public IBrowser Browser { get; set; }
    }

