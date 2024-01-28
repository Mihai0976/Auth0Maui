using Auth0Maui.Domain.Models.Results;
using MediatR;
using Microsoft.Azure.Documents;
using Microsoft.VisualStudio.Services.UserAccountMapping;

namespace AuthoMaui.Aplication.Models.Commands.UserManagement;

public class CreateUserCommand : IRequest<CommandResult<User>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}