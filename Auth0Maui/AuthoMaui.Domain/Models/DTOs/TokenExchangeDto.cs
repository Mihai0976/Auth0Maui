using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Models.DTOs
{
    internal class TokenExchangeDto
    {
        public string AccessToken { get; set; }
        public string IdentityToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTimeOffset AccessTokenExpiration { get; set; }
    }
}
