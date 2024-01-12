using AuthoMaui.Domain.Entities;
using AuthoMaui.Domain.Entities.UserManagement;
using AuthoMaui.Domain.Models.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Models.Commands.UserManagement
{
    public class CreateUserCommand: IRequest<CommandResult<User>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
