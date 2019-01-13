using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Authenticaton.Contracts.CookieAuthentication;
using Authenticaton.Contracts.JwtAuthentication;
using System;

namespace Authentication.AppServices.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal principal)
        {
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return null;
            }
            switch (principal.Identity.AuthenticationType)
            {
                case CookieAuthenticationDefaults.AuthenticationScheme:
                    return principal.Claims.FirstOrDefault(c => c.Type == CookieCustomClaimNames.UserName)?.Value;
                case JwtBearerDefaults.AuthenticationScheme:
                    return principal.Claims.FirstOrDefault(c => c.Type == JwtCustomClaimNames.UserName)?.Value;
                default:
                    return null;
            }
        }
        public static Guid? GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return null;
            }
            switch (principal.Identity.AuthenticationType)
            {
                case CookieAuthenticationDefaults.AuthenticationScheme:
                    var cookieId = principal.Claims.FirstOrDefault(c => c.Type == CookieCustomClaimNames.UserId)?.Value;
                    return Guid.TryParse(cookieId, out var cUserId) ? cUserId : (Guid?)null;
                case JwtBearerDefaults.AuthenticationScheme:
                    var jwtId = principal.Claims.FirstOrDefault(c => c.Type == JwtCustomClaimNames.UserId)?.Value;
                    return Guid.TryParse(jwtId, out var jUserId) ? jUserId : (Guid?)null;
                default:
                    return null;
            }
        }
        public static string GetAuthToken(this ClaimsPrincipal principal)
        {
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return null;
            }
            switch (principal.Identity.AuthenticationType)
            {
                case CookieAuthenticationDefaults.AuthenticationScheme:
                    return principal.Claims.FirstOrDefault(c => c.Type == CookieCustomClaimNames.AuthToken)?.Value;
                default:
                    return null;
            }
        }
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return null;
            }
            switch (principal.Identity.AuthenticationType)
            {
                case CookieAuthenticationDefaults.AuthenticationScheme:
                    var cookieEmail = principal.Claims.FirstOrDefault(c => c.Type == CookieCustomClaimNames.Email)?.Value;
                    return cookieEmail;
                case JwtBearerDefaults.AuthenticationScheme:
                    var jwtEmail = principal.Claims.FirstOrDefault(c => c.Type == JwtCustomClaimNames.Email)?.Value;
                    return jwtEmail;
                default:
                    return null;
            }
        }
        public static string GetUserPhone(this ClaimsPrincipal principal)
        {
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return null;
            }
            switch (principal.Identity.AuthenticationType)
            {
                case CookieAuthenticationDefaults.AuthenticationScheme:
                    var cookiePhone = principal.Claims.FirstOrDefault(c => c.Type == CookieCustomClaimNames.Phone)?.Value;
                    return cookiePhone;
                case JwtBearerDefaults.AuthenticationScheme:
                    var jwtPhone = principal.Claims.FirstOrDefault(c => c.Type == JwtCustomClaimNames.Phone)?.Value;
                    return jwtPhone;
                default:
                    return null;
            }
        }
        public static string GetUserFIO(this ClaimsPrincipal principal)
        {
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return null;
            }
            switch (principal.Identity.AuthenticationType)
            {
                case CookieAuthenticationDefaults.AuthenticationScheme:
                    var cookieFIO = principal.Claims.FirstOrDefault(c => c.Type == CookieCustomClaimNames.FIO)?.Value;
                    return cookieFIO;
                case JwtBearerDefaults.AuthenticationScheme:
                    var jwtFIO = principal.Claims.FirstOrDefault(c => c.Type == JwtCustomClaimNames.FIO)?.Value;
                    return jwtFIO;
                default:
                    return null;
            }
        }


    }
}
