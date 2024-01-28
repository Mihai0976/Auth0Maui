using Auth0maui.Domain.Entities.UserManagement;
using Auth0Maui.Domain.Models.Results;
using MediatR;

namespace Auth0Maui.Domain.Models.Commands;

public class CreateUserCommand : IRequest<CommandResult<User>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}