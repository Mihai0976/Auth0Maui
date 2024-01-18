using IdentityModel.Client;
using IdentityModel.OidcClient.Browser;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace Auth0Maui.MAUI.Auth0;

public class WebBrowserAuthenticator : IBrowser
{
    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await WebAuthenticator.Default.AuthenticateAsync(
                new Uri(options.StartUrl),
                new Uri(options.EndUrl));

            var url = new RequestUrl(options.EndUrl)
                .Create(new Parameters(result.Properties));

            // FB adding #_=_ to the end of the url, so we need to remove it
            if (url.EndsWith("%23_%3D_")) url = url.Substring(0, url.LastIndexOf("%23_%3D_"));

            return new BrowserResult
            {
                Response = url,
                ResultType = BrowserResultType.Success
            };
        }
        catch (TaskCanceledException)
        {
            return new BrowserResult
            {
                ResultType = BrowserResultType.UserCancel,
                ErrorDescription = "Login canceled by the user."
            };
        }
    }
}