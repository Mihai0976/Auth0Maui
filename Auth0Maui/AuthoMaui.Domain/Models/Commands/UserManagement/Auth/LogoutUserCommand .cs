using AuthoMaui.Domain.Models.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Models.Commands.UserManagement.Auth
{
    public class LogoutUserCommand :IRequest<CommandResult<bool>>
    {
        public Guid UserId { get; set; }
    }
}
