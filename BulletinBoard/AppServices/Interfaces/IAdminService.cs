using BulletinDomain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace AppServices.Interfaces
{
    public interface IAdminService
    {
        List<UserDto> GetAllUsersAsync();
        ApiResult<UserDto> MakeAdmin(Guid id);
        ApiResult<UserDto> UnMakeAdmin(Guid id);
        Task <ApiResult<UserDto>> banUser(Guid id);

    }
}
