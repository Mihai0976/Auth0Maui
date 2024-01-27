
using Auth0Maui.Domain.Models.DTOs;

namespace Auth0Maui.MAUI.ViewModels;

public partial class AccountViewModel : BaseViewModel
{
    private UserDto _currentUser;
    public UserDto CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }

    public void InitializeWithUserData(UserDto userData)
    {
        CurrentUser = userData;
    }
}
