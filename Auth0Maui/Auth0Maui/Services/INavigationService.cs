
namespace Auth0Maui.Services;

public interface INavigationService
{
    Task NavigateToAsync(string route);
    Task NavigateBackAsync();
    Task NavigateToModalAsync(Page modalPage);
    Task CloseModalAsync();
}

