using Android.App;
using Microsoft.Maui.Authentication;
using Android.Content;


namespace Auth0Maui.MAUI.Platforms.Android;

[Activity(NoHistory = true, Name = "com.companyname.Auth0Maui.MAUI.Auth0CallbackActivity",  Exported = true)]
[IntentFilter(new[] { Intent.ActionView },
               Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
               DataScheme = "myapp", // This should match the scheme in your RedirectUri
               DataHost = "callback")] // This should match the host in your RedirectUri
public class Auth0CallbackActivity : WebAuthenticatorCallbackActivity
{
}
