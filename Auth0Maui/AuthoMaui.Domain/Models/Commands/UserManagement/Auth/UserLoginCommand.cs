using AuthoMaui.Domain.Models.Results;
using MediatR;


namespace AuthoMaui.Domain.Models.Commands.UserManagement.Auth
{
    public class UserLoginCommand : IRequest<CommandResult<AuthResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
