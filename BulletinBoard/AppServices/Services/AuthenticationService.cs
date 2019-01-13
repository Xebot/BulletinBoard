using AppServices.Interfaces;
using BulletinDomain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;
using Microsoft.Extensions.Identity;

namespace AppServices.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<User> _signInManager;
        

        
        public AuthenticationService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            
        }

        public async Task<SignInResult> SignInUserAsync(UserLoginDto loginUserRequest)
        {
            if (loginUserRequest == null)
            {
                throw new ArgumentNullException(nameof(loginUserRequest));
            }           

            var result = await _signInManager.PasswordSignInAsync(loginUserRequest.Email, loginUserRequest.Password,
                                                                  isPersistent: false, lockoutOnFailure: false);
            
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Некорректный логин или пароль");
            }
            else return result;
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        

    }
}
