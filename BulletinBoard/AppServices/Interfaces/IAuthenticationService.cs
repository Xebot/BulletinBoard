using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace AppServices.Interfaces
{
    public interface IAuthenticationService
    {
        Task<SignInResult> SignInUserAsync(UserLoginDto loginUserRequest);

        Task SignOutUserAsync();
    }
}
