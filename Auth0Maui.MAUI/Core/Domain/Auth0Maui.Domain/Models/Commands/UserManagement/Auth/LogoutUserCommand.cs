using MediatR;
using Auth0Maui.Domain.Models.Results;

namespace ReFeelApp.Common.Domain.Models.Commands.UserManagement.Auth;

public class LogoutUserCommand : IRequest<CommandResult<bool>>
{
    public Guid UserId { get; set; }
}