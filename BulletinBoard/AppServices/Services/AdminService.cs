using AutoMapper;
using AppServices.Interfaces;
using BulletinDomain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace AppServices.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public AdminService(UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;            
            _mapper = mapper;
            _roleManager = roleManager;
        }        

        public async Task<ApiResult<UserDto>> CreateUserAsync(CreateUserDto createUserRequest)
        {
            if (createUserRequest == null)
            {
                throw new ArgumentNullException(nameof(createUserRequest));
            }
            var newUser = _mapper.Map<UserDto>(createUserRequest);            
            string resultString = string.Join(string.Empty, Regex.Matches(createUserRequest.Phone, @"\d+").OfType<Match>().Select(m => m.Value));
            newUser.UserTel = resultString;

            var entityUser = _mapper.Map<User>(newUser);
            var result = _userManager.CreateAsync(entityUser, createUserRequest.Password).Result;
            
            //Создание Ролей
            var role = new IdentityRole<Guid>("User");            
            var test = await _roleManager.CreateAsync(role);
            var res1 = await _userManager.AddToRoleAsync(entityUser, role.Name);

            //Присваивание роли Администратора
            //var res1 = await _userManager.AddToRoleAsync(entityUser, "Admin");

            List<string> err = new List<string>();            
            foreach (var i in result.Errors)
            {                
                err.Add(i.Description);
            }                             

            if (!result.Succeeded)
            {
                return new ApiResult<UserDto>(result: null, isSuccess:false, errors:err.ToArray());
            }
            var user = _mapper.Map<UserDto>(entityUser);
            return new ApiResult<UserDto>(user, isSuccess:true, errors:null);
        }

        public async Task<ApiResult<UserDto>> GetUserAsync(Guid id)
        {
            string UserId = Convert.ToString(id);
            var user = await _userManager.FindByIdAsync(UserId);
            if(user == null)
            {
                return new ApiResult<UserDto>(result:null, isSuccess:false, errors: new[] { "Пользователь с таким Id не найден","" });
            }
            return new ApiResult<UserDto> (result:_mapper.Map<UserDto>(user), isSuccess:true, errors : null);
        }
        public async Task<ActionResult> UpdateUserAsync(UserDto user)
        {
            
            string UserId = Convert.ToString(user.Id);
            var oldUser = await _userManager.FindByIdAsync(UserId);
            if (oldUser == null)
            {
                return null;
            }
            string resultString = string.Join(string.Empty, Regex.Matches(user.UserTel, @"\d+").OfType<Match>().Select(m => m.Value));
            oldUser.PhoneNumber = resultString;
            oldUser.Email = user.UserEmail;
            oldUser.RegionId = user.RegionId;
            oldUser.UserAdress = user.UserAdress;
            oldUser.UserName = user.UserName;
            await _userManager.UpdateAsync(oldUser);
            

            OkResult ok = new OkResult();
            return ok;
        }

        public async Task<ApiResult<User>> GetUserRole(Guid id)
        {
            string UserId = Convert.ToString(id);
            var user = _userManager.FindByIdAsync(UserId).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            return new ApiResult<User>(result: user, isSuccess: true, errors: null);
        }

        public List<UserDto> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var listUsers = _mapper.Map<List<UserDto>>(users);
            foreach (var t in users)
            {
                var role = _userManager.GetRolesAsync(t).Result;
                listUsers.FirstOrDefault(x => x.Id == t.Id).UserRole = role;
            }           
            
            return listUsers;
        }

        public ApiResult<UserDto> MakeAdmin(Guid id)
        {
            string Id = Convert.ToString(id);
            var entityUser = _userManager.FindByIdAsync(Id).Result;
            if (entityUser == null)
            {
                return new ApiResult<UserDto>(result: null, isSuccess: false, errors: new string[] {"Пользователь не найден"});
            }
            var res1 =_userManager.AddToRoleAsync(entityUser, "Admin").Result;
            if (res1 != null && res1.Succeeded)
            {
                return new ApiResult<UserDto>(result: _mapper.Map<UserDto>(entityUser), isSuccess: true, errors: null);
            }
            else
            {
                return new ApiResult<UserDto>(result: null, isSuccess: false, errors: new string[] { "Не удалось назначить роль" });
            }

        }

        public ApiResult<UserDto> UnMakeAdmin(Guid id)
        {
            string Id = Convert.ToString(id);
            var entityUser = _userManager.FindByIdAsync(Id).Result;
            if (entityUser == null)
            {
                return new ApiResult<UserDto>(result: null, isSuccess: false, errors: new string[] { "Пользователь не найден" });
            }
            var res1 = _userManager.RemoveFromRoleAsync(entityUser, "Admin").Result;
            
            if (res1.Succeeded)
            {
                return new ApiResult<UserDto>(result: _mapper.Map<UserDto>(entityUser), isSuccess: true, errors: null);
            }
            else
            {
                return new ApiResult<UserDto>(result: null, isSuccess: false, errors: new string[] { "Не удалось назначить роль" });
            }

        }

        public async Task<ApiResult<UserDto>> banUser(Guid id)
        {
            string Id = Convert.ToString(id);
            var entityUser = await _userManager.FindByIdAsync(Id);
            if (entityUser == null)
            {
                return new ApiResult<UserDto>(result: null, isSuccess: false, errors: new string[] { "Пользователь не найден" });
            }
            if (entityUser.UserStatus == "banned")
            {
                entityUser.UserStatus = "unbanned";
                await _userManager.UpdateAsync(entityUser);
            }
            else
            {
                entityUser.UserStatus = "banned";
                await _userManager.UpdateAsync(entityUser);
            }
            return new ApiResult<UserDto>(result: _mapper.Map<UserDto>(entityUser), isSuccess: true, errors: null);
        }

    }
}
