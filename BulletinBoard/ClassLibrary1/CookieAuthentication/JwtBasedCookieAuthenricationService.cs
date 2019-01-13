using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using Authentication.AppServices.JwtAuthentication;
using Authenticaton.Contracts;
using Authenticaton.Contracts.Basic;
using Authenticaton.Contracts.JwtAuthentication.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Authenticaton.Contracts.JwtAuthentication;
using Authenticaton.Contracts.CookieAuthentication;
using WebApi.Contracts.DTO;
using AppServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.AppServices.CookieAuthentication
{
    public class JwtBasedCookieAuthenricationService : IDisposable, IJwtBasedCookieAuthenricationService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IJwtTokenService _tokenService;
        private readonly HttpClient _httpClient;
        private readonly JwtClientAuthenticatonOptions _jwtOptions;
        
        public JwtBasedCookieAuthenricationService(
            IHttpContextAccessor contextAccessor,
            IOptions<JwtClientAuthenticatonOptions> authOptions,
            IJwtTokenService tokenService)
        {
            _contextAccessor = contextAccessor;
            _tokenService = tokenService;
            _jwtOptions = authOptions.Value;
            _httpClient = new HttpClient();   
        }
        public async Task<AuthenticationResult> SignInAsync(BasicAuthenticationRequest request)
        {
            var context = _contextAccessor.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("No http context provided");
            }
            var response = await _httpClient.PostAsJsonAsync("http://localhost:58886/authentication/jwt", request); 
            if (!response.IsSuccessStatusCode)
            {
                var read = response.Content.ReadAsAsync<string[]>();
                return AuthenticationResult.Failed(read.Result);                
            }
            var token = await response.Content.ReadAsAsync<JwtAuthenticationToken>();
            if (token == null || token.AuthToken == null)
            {
                return AuthenticationResult.Failed("Token is null");
            }
            var cookieIdentity = new ClaimsIdentity(new[]
            {
                new Claim(CookieCustomClaimNames.AuthToken, token.AuthToken),
                new Claim(CookieCustomClaimNames.UserId, token.UserId.ToString()),
                
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var jwtClaims = _tokenService.GetClaims(token.AuthToken);
            var userNameClaim = jwtClaims?.FirstOrDefault(c => c.Type == JwtCustomClaimNames.UserName);
            var userEmailClaim = jwtClaims?.FirstOrDefault(c => c.Type == JwtCustomClaimNames.Email);
            var userPhoneClaim = jwtClaims?.FirstOrDefault(c => c.Type == JwtCustomClaimNames.Phone);
            var userFIOClaim = jwtClaims?.FirstOrDefault(c => c.Type == JwtCustomClaimNames.FIO);
            var userRoleClaim = jwtClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (userNameClaim != null)
            {
                cookieIdentity.AddClaim(new Claim(CookieCustomClaimNames.UserName, userNameClaim.Value));
            }
            if (userEmailClaim != null)
            {
                cookieIdentity.AddClaim(new Claim(CookieCustomClaimNames.Email, userEmailClaim.Value));
            }
            if (userPhoneClaim != null)
            {
                cookieIdentity.AddClaim(new Claim(CookieCustomClaimNames.Phone, userPhoneClaim.Value));
            }
            if (userRoleClaim != null)
            {
                cookieIdentity.AddClaim(new Claim(ClaimTypes.Role, userRoleClaim.Value));
            }
            if (userFIOClaim != null)
            {
                cookieIdentity.AddClaim(new Claim(CookieCustomClaimNames.FIO, userFIOClaim.Value));
            }
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(cookieIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = token.Expires
                });
            return AuthenticationResult.Succeed;
        
        }



        public async Task<AuthenticationResult> SignInExternalAsync(string UserEmail)
        {
            var context = _contextAccessor.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("No http context provided");
            }
            var response = await _httpClient.PostAsJsonAsync("http://localhost:58886/authentication/jwt/ext", UserEmail);
            if (!response.IsSuccessStatusCode)
            {
                var read = response.Content.ReadAsAsync<string[]>();
                return AuthenticationResult.Failed(read.Result);
            }
            var token = await response.Content.ReadAsAsync<JwtAuthenticationToken>();
            if (token == null || token.AuthToken == null)
            {
                return AuthenticationResult.Failed("Token is null");
            }
            var cookieIdentity = new ClaimsIdentity(new[]
            {
                new Claim(CookieCustomClaimNames.AuthToken, token.AuthToken),
                new Claim(CookieCustomClaimNames.UserId, token.UserId.ToString()),

            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var jwtClaims = _tokenService.GetClaims(token.AuthToken);
            var userNameClaim = jwtClaims?.FirstOrDefault(c => c.Type == JwtCustomClaimNames.UserName);
            var userEmailClaim = jwtClaims?.FirstOrDefault(c => c.Type == JwtCustomClaimNames.Email);
            var userPhoneClaim = jwtClaims?.FirstOrDefault(c => c.Type == JwtCustomClaimNames.Phone);
            var userFIOClaim = jwtClaims?.FirstOrDefault(c => c.Type == JwtCustomClaimNames.FIO);
            var userRoleClaim = jwtClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (userNameClaim != null)
            {
                cookieIdentity.AddClaim(new Claim(CookieCustomClaimNames.UserName, userNameClaim.Value));
            }
            if (userEmailClaim != null)
            {
                cookieIdentity.AddClaim(new Claim(CookieCustomClaimNames.Email, userEmailClaim.Value));
            }
            if (userPhoneClaim != null)
            {
                cookieIdentity.AddClaim(new Claim(CookieCustomClaimNames.Phone, userPhoneClaim.Value));
            }
            if (userFIOClaim != null)
            {
                cookieIdentity.AddClaim(new Claim(CookieCustomClaimNames.FIO, userFIOClaim.Value));
            }
            if (userRoleClaim != null)
            {
                cookieIdentity.AddClaim(new Claim(ClaimTypes.Role, userRoleClaim.Value));
            }        
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(cookieIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = token.Expires
                });
            return AuthenticationResult.Succeed;

        }

        public async Task SignOutAsync()
        {
            var context = _contextAccessor.HttpContext;
            if (context == null)
                throw new InvalidOperationException("No http context provided");
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        public void Dispose()
        {
            _httpClient?.Dispose();
        }

       
    }
}
