
using System.ComponentModel.DataAnnotations;


namespace Auth0Maui.Domain.Models.DTOs
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
