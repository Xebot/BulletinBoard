using BulletinDomain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace AppServices.Interfaces
{
    public interface IUserService
    {
        Task<ApiResult<UserDto>> CreateUserAsync(CreateUserDto createUserRequest);
        Task<ApiResult<UserDto>> GetUserAsync(Guid id);
        Task<ActionResult> UpdateUserAsync(UserDto user);
        Task<ApiResult<User>> GetUserRole(Guid id);
        Task<string> Confirm(string userId, string code);
        Task<ApiResult<string>> SendEmail(string Email, string callbackUrl);
        Task<ApiResult<UserDto>> CreateUserExternalAsync(TokenDto token);
        Task<ApiResult<UserDto>> GetUserByEmailAsync(string email);
    }
}
