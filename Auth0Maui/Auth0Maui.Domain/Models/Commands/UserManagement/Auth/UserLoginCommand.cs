using Auth0Maui.Domain.Models.Results;
using MediatR;


namespace Auth0Maui.Domain.Models.Commands.UserManagement.Auth
{
    public class UserLoginCommand : IRequest<CommandResult<AuthResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
