using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Models.DTOs
{
    public class LoginDto
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Identifier")]
        public string Identifier { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
