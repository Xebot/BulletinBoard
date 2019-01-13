using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Authenticaton.Contracts.JwtAuthentication;
using BulletinDomain.Entities;
using Authenticaton.Contracts.JwtAuthentication.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.AppServices.JwtAuthentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly JwtBaseAuthenticationOptions _authenticationOptions;
        

        public JwtTokenService(IOptions<JwtBaseAuthenticationOptions> authenticationOprions)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _authenticationOptions = authenticationOprions.Value;
           
        }

        public JwtAuthenticationToken CreateToken(User user, TimeSpan lifeTime, IList<string> roles)
        {
            
            var descriptor = new SecurityTokenDescriptor
            {
                Audience = _authenticationOptions.Audience,
                Issuer = _authenticationOptions.Issuer,
                Expires = DateTime.UtcNow.Add(lifeTime),
                SigningCredentials = JwtDefaultsProvider.GetSigningCredentials(_authenticationOptions.Secret),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")),
                    new Claim(JwtCustomClaimNames.UserId, user.Id.ToString()),
                    new Claim(JwtCustomClaimNames.UserName, user.UserName),
                    new Claim(JwtCustomClaimNames.Email, user.Email),
                    new Claim(JwtCustomClaimNames.FIO, user.FIO),
                    new Claim(JwtCustomClaimNames.Phone, Convert.ToInt64(user.PhoneNumber).ToString("+#(###)###-##-##"))
                }, JwtBearerDefaults.AuthenticationScheme)      
                
            };
            foreach (var role in roles)
            {
                descriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            var token = _tokenHandler.CreateToken(descriptor);
            return new JwtAuthenticationToken
            {
                AuthToken = _tokenHandler.WriteToken(token),
                Expires = descriptor.Expires,
                UserId = user.Id
            };
        }
        public Claim[] GetClaims(string authToken)
        {
            if (string.IsNullOrEmpty(authToken))
            {
                throw new ArgumentException(nameof(authToken));
            }
            if (!_tokenHandler.CanReadToken(authToken))
            {
                throw new ArgumentException("Не могу прочитать JWT токен");
            }
            try
            {
                var validationParams = JwtDefaultsProvider.GetTokenValidationParameters(
                    _authenticationOptions.Issuer,
                    _authenticationOptions.Audience,
                    _authenticationOptions.Secret);
                var principal = _tokenHandler.ValidateToken(authToken, validationParams, out _);
                return principal.Claims.ToArray();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
