using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Authentication.AppServices.JwtAuthentication;
using Authenticaton.Contracts.Basic;
using Authenticaton.Contracts.JwtAuthentication;
using Authenticaton.Contracts.JwtAuthentication.Options;
using BulletinDomain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApi.Contracts.DTO;

namespace Authentication.AppServices.JwtAuthentication
{
    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtServerAuthenticationOptions _authenticationOptions;
        private readonly IJwtTokenService _tokenService;
        

        public JwtAuthenticationService(UserManager<User> userManager, IJwtTokenService tokenService, IOptions<JwtServerAuthenticationOptions> authenticationOptions)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _authenticationOptions = authenticationOptions.Value;
        }
        public async Task<JwtAuthenticationResult> AuthenticateAsync(BasicAuthenticationRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var identity = await _userManager.FindByEmailAsync(request.Email);
            if (identity == null)
            {
                return JwtAuthenticationResult.Failed("Пользователь с таким логином не найден");
            }

            if (identity.UserStatus == "banned")
            {
                return JwtAuthenticationResult.Failed("Пользователь забанен. Свяжитесь с администратором для выяснения причин");
            }
            if (identity.EmailConfirmed == false)
            {
                return JwtAuthenticationResult.Failed("Подтвердите свой email");
            }
            
            var isPasswordMatched = await _userManager.CheckPasswordAsync(identity, request.Password);
            if (!isPasswordMatched)
            {
                return JwtAuthenticationResult.Failed("Неправильное имя пользователя или пароль");
            }
            var roles = _userManager.GetRolesAsync(identity).Result;          
            
            var token = _tokenService.CreateToken(identity, _authenticationOptions.Lifetime, roles);
            return JwtAuthenticationResult.Succeed(token);
        }
        public async Task<JwtAuthenticationResult> AuthenticateExternalAsync(string UserEmail)
        {
            if (UserEmail == null)
            {
                throw new ArgumentNullException(UserEmail);
            }
            var identity = await _userManager.FindByEmailAsync(UserEmail);
            if (identity == null)
            {
                return JwtAuthenticationResult.Failed("Пользователь с таким логином не найден");
            }
            if (identity.UserStatus == "banned")
            {
                return JwtAuthenticationResult.Failed("Пользователь забанен. Свяжитесь с администратором для выяснения причин");
            }
            if (identity.EmailConfirmed == false)
            {
                return JwtAuthenticationResult.Failed("Подтвердите свой email");
            }
            if (identity == null)
            {
                return JwtAuthenticationResult.Failed("Пользователь с таким логином не найден");
            }            
            var roles = _userManager.GetRolesAsync(identity).Result;

            var token = _tokenService.CreateToken(identity, _authenticationOptions.Lifetime, roles);
            return JwtAuthenticationResult.Succeed(token);
        }
    }
}
