using Auth0maui.Domain.Entities.UserManagement;
using MediatR;


namespace Auth0Maui.Domain.Models.Commands;
public class ValidateCredentialsCommand : IRequest<User>
{
    public string Identity { get; set; }
    public string Password { get; set; }
}
