using AuthoMaui.Domain.Entities.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Models.DTOs
{
     public class SwitchModeDto
    {
        public string? UserId { get; set; }
        public Mode NewMode { get; set; }
    }
}

