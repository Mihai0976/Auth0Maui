
namespace Auth0Maui.Domain.Models.Commands;

public class ValidateUserCredentialsCommand
{
    public string Identity { get; set; }
    public string Password { get; set; }
}
