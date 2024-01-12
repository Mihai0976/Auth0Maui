using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Models.DTOs
{
    public class UpdateProfileDto
    {
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        // other properties as needed, e.g. Name, Address etc.
    }
}
