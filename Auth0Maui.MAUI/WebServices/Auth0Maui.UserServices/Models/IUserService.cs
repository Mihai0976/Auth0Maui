namespace Auth0Maui.UserServices.Models
{
    public interface IUserService
    {
        AuthenticationItem GetUserById(int id);
        bool ValidateUserCredentials(string username, string password);

    }
}
