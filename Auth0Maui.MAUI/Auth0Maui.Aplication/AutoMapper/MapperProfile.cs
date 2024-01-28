using Auth0maui.Domain.Entities.UserManagement;
using Auth0Maui.Domain.Entities.UserManagement;
using Auth0Maui.Domain.Models.Commands.UserManagement.Auth;
using AuthoMaui.Aplication.Models.DTOs;
using AuthoMaui.Domain.Models.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth0Maui.Aplication.AutoMapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserRegistrationDto>()
            .AfterMap((src, dest) => MapUserRegistrationDto(src, dest));

        // Mapping from RegisterUserCommand to User
        CreateMap<RegisterUserCommand, User>()
            .ForMember(dest => dest.UserProfile, act => act.MapFrom(src => src));

        // Mapping from RegisterUserCommand to UserProfile
        CreateMap<RegisterUserCommand, UserProfile>()
            .ForMember(dest => dest.ExternalInfo, act => act.MapFrom(src => src));

        // Mapping from RegisterUserCommand to ExternalInfo
        CreateMap<RegisterUserCommand, ExternalInfo>();

        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserProfile, opt => opt.MapFrom(src => src));

        // Mapping from UpdateUserDto to UserProfile
        CreateMap<UpdateUserDto, UserProfile>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.CountryPrefix, opt => opt.MapFrom(src => src.CountryPrefix))
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId.HasValue ? src.UserId.Value : Guid.Empty));

        CreateMap<User, AuthoMaui.Aplication.Models.DTOs.UserDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserProfile.Name))
            .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.UserProfile.FamilyName))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.UserProfile.PictureUrl));


        CreateMap<ClaimsIdentity, User>()
            .ForMember(dest => dest.Email, opts => opts.MapFrom(src => GetClaimValue(src.Claims, ClaimTypes.Email)))
            .ForMember(dest => dest.ExternalUser, opts => opts.MapFrom(src => true))
            .ForMember(dest => dest.Role, opts => opts.MapFrom(src => UserRole.Basic))
            .ForMember(dest => dest.UserProfile, opts => opts.MapFrom(src => src.Claims))
            .AfterMap((src, dest) =>
            {
                MapUserClaims(src.Claims, dest);
                if (dest.UserProfile != null && dest.UserProfile.ExternalInfo != null)
                    dest.UserProfile.ExternalInfo.IsAuthenticated = src.IsAuthenticated;
            });


        CreateMap<IEnumerable<Claim>, UserProfile>()
            .ForMember(dest => dest.Nickname, opts => opts.MapFrom(src => GetClaimValue(src, "nickname")))
            .ForMember(dest => dest.Name, opts => opts.MapFrom(src => GetClaimValue(src, "name")))
            .ForMember(dest => dest.PictureUrl, opts => opts.MapFrom(src => GetClaimValue(src, "picture")))
            .ForMember(dest => dest.Locale, opts => opts.MapFrom(src => GetClaimValue(src, "locale")))
            .ForMember(dest => dest.GivenName,
                opts => opts.MapFrom(src => GetClaimValue(src, ClaimTypes.GivenName)))
            .ForMember(dest => dest.FamilyName, opts => opts.MapFrom(src => GetClaimValue(src, ClaimTypes.Surname)))
            .ForMember(dest => dest.ExternalInfo, opts => opts.MapFrom(src => src))
            .ForMember(dest => dest.EmailVerified,
                opts => opts.MapFrom(src => GetClaimValue(src, "email_verified") == "true"));

        CreateMap<IEnumerable<Claim>, ExternalInfo>()
            .ForMember(dest => dest.Issuer, opts => opts.MapFrom(src => GetClaimValue(src, "iss")))
            .ForMember(dest => dest.Audience, opts => opts.MapFrom(src => GetClaimValue(src, "aud")))
            .ForMember(dest => dest.SessionId, opts => opts.MapFrom(src => GetClaimValue(src, "sid")))
            .ForMember(dest => dest.ExternalIdentifier,
                opts => opts.MapFrom(src => GetClaimValue(src, ClaimTypes.NameIdentifier)))
            .ForMember(dest => dest.IssuedAt, opts => opts.MapFrom(src => ConvertToLong(GetClaimValue(src, "iat"))))
            .ForMember(dest => dest.Expiration, opts => opts.MapFrom(src => ConvertToLong(GetClaimValue(src, "exp"))));

        CreateMap<User, RegisterUserCommand>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.Equals(Guid.Empty.ToString()))
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.UserProfile.ExternalInfo.AccessToken))
            .ForMember(dest => dest.AccessTokenExpiration,
                opt => opt.MapFrom(src => src.UserProfile.ExternalInfo.AccessTokenExpiration))
            ;

        CreateMap<UserRegistrationDto, RegisterUserCommand>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password,
                opt => opt.Ignore()) // Password is ignored as it's not in UserRegistrationDto
            .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Nickname))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.PictureUrl))
            .ForMember(dest => dest.Locale, opt => opt.MapFrom(src => src.Locale))
            .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.GivenName))
            .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.FamilyName))
            .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => src.EmailVerified))
            .ForMember(dest => dest.Issuer, opt => opt.Ignore()) // Assuming no Issuer field in UserRegistrationDto
            .ForMember(dest => dest.Audience, opt => opt.Ignore()) // Assuming no Audience field in UserRegistrationDto
            .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.SessionId))
            .ForMember(dest => dest.ExternalIdentifier,
                opt => opt.Ignore()) // Assuming no ExternalIdentifier field in UserRegistrationDto
            .ForMember(dest => dest.IssuedAt, opt => opt.Ignore()) // Assuming no IssuedAt field in UserRegistrationDto
            .ForMember(dest => dest.Expiration,
                opt => opt.Ignore()) // Assuming no Expiration field in UserRegistrationDto
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken))
            .ForMember(dest => dest.AccessTokenExpiration, opt => opt.MapFrom(src => src.AccessTokenExpiration))
            .ForMember(dest => dest.IdentityToken, opt => opt.MapFrom(src => src.IdentityToken))
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
            .ForMember(dest => dest.ExpiresIn,
                opt => opt.Ignore()) // Assuming no ExpiresIn field in UserRegistrationDto
            .ForMember(dest => dest.Scope, opt => opt.Ignore()) // Assuming no Scope field in UserRegistrationDto
            .ForMember(dest => dest.TokenType,
                opt => opt.Ignore()) // Assuming no TokenType field in UserRegistrationDto
            .ForMember(dest => dest.IsAuthenticated, opt => opt.MapFrom(src => src.IsAuthenticated));
    }

    private static string GetClaimValue(IEnumerable<Claim> claims, string claimType)
    {
        return claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? string.Empty;
    }

    private static long ConvertToLong(string value)
    {
        return long.TryParse(value, out var result) ? result : 0;
    }

    private static DateTime ConvertToDateTime(string value)
    {
        return DateTime.TryParse(value, out var result) ? result : DateTime.MinValue;
    }

    private void MapUserClaims(IEnumerable<Claim> claims, User user)
    {
        var claimList = claims.ToList();
        user.Email = GetClaimValue(claimList, ClaimTypes.Email);
        user.Role = UserRole.Basic; // Assuming default role
        user.UserProfile = new UserProfile
        {
            Nickname = GetClaimValue(claimList, "nickname"),
            Name = GetClaimValue(claimList, "name"),
            PictureUrl = GetClaimValue(claimList, "picture"),
            Locale = GetClaimValue(claimList, "locale"),
            GivenName = GetClaimValue(claimList, ClaimTypes.GivenName),
            FamilyName = GetClaimValue(claimList, ClaimTypes.Surname),
            UpdatedAt = ConvertToDateTime(GetClaimValue(claimList, "updated_at")),
            EmailVerified = GetClaimValue(claimList, "email_verified") == "true",
            ExternalInfo = new ExternalInfo
            {
                Issuer = GetClaimValue(claimList, "iss"),
                Audience = GetClaimValue(claimList, "aud"),
                SessionId = GetClaimValue(claimList, "sid"),
                ExternalIdentifier = GetClaimValue(claimList, ClaimTypes.NameIdentifier),
                IssuedAt = ConvertToLong(GetClaimValue(claimList, "iat")),
                Expiration = ConvertToLong(GetClaimValue(claimList, "exp"))
            }
        };
    }

    private void MapUserRegistrationDto(User source, UserRegistrationDto destination)
    {
        if (source.UserProfile != null)
        {
            destination.Nickname = source.UserProfile.Nickname;
            destination.Name = source.UserProfile.Name;
            destination.PictureUrl = source.UserProfile.PictureUrl;
            destination.Locale = source.UserProfile.Locale;
            destination.GivenName = source.UserProfile.GivenName;
            destination.FamilyName = source.UserProfile.FamilyName;
            destination.EmailVerified = source.UserProfile.EmailVerified;

            if (source.UserProfile.ExternalInfo != null)
            {
                destination.AccessToken = source.UserProfile.ExternalInfo.AccessToken;
                destination.AccessTokenExpiration = source.UserProfile.ExternalInfo.AccessTokenExpiration;
                destination.IdentityToken = source.UserProfile.ExternalInfo.IdentityToken;
                destination.RefreshToken = source.UserProfile.ExternalInfo.RefreshToken;
                destination.SessionId = source.UserProfile.ExternalInfo.SessionId;
                destination.IsAuthenticated = source.UserProfile.ExternalInfo.IsAuthenticated;
            }
        }
    }
}