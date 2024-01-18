﻿using AuthoMaui.Domain.Entities.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthoMaui.Domain.Entities.UserManagement
{
    public class ExternalInfo:AuditableEntity
    {
        public Guid UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }
        public string? ExternalIdentifier { get; set; } // e.g., "sub" claim

        public string? SessionId { get; set; } // "sid" claim
        public string? AccessToken { get; set; }

        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public long IssuedAt { get; set; }
        public DateTimeOffset? AccessTokenExpiration { get; set; }
        public string? IdentityToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Scope { get; set; }
        public string? TokenType { get; set; }
        public long Expiration { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}