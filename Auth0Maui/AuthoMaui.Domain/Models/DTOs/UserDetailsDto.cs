using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Models.DTOs
{
    internal class UserDetailsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string UserPictureUrl { get; set; }
        public string CountryFlagUrl { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
