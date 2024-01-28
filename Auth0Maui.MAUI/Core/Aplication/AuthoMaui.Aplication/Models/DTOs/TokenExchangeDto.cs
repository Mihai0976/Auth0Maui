using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Aplication.Models.DTOs;

public class TokenExchangeDto
{
    public string AccessToken { get; set; }
    public string IdentityToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset AccessTokenExpiration { get; set; }
}