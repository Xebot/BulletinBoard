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
using AppServices.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AppServices.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IEmailSender _emailSender;
        

        public UserService(UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole<Guid>> roleManager, IEmailSender emailSender)
        {
            _userManager = userManager;            
            _mapper = mapper;
            _roleManager = roleManager;
            _emailSender = emailSender;            
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
            var result = await _userManager.CreateAsync(entityUser, createUserRequest.Password);

            //Создание Ролей
            var role = new IdentityRole<Guid>("User");
            var test = await _roleManager.CreateAsync(role);
            var res1 = await _userManager.AddToRoleAsync(entityUser, role.Name);

            //Роль админа для первородного Админа!
            //var role = new IdentityRole<Guid>("Admin");
            //var test = await _roleManager.CreateAsync(role);
            //var res1 = await _userManager.AddToRoleAsync(entityUser, role.Name);

            var code = _userManager.GenerateEmailConfirmationTokenAsync(entityUser).Result;

            //await _userManager.UpdateAsync(entityUser);
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
            return new ApiResult<UserDto>(user, isSuccess:true, errors:new string[] { code });
        }
        public async Task<ApiResult<UserDto>> CreateUserExternalAsync(TokenDto token)
        {

            UserDto user = new UserDto
            {
                UserEmail = token.email,
                UserName = token.email,
                FIO = token.userName
            };
            
            var entityUser = _mapper.Map<User>(user);
            var result = await _userManager.CreateAsync(entityUser);

            //Создание Ролей
            var role = new IdentityRole<Guid>("User");
            var test = await _roleManager.CreateAsync(role);
            var res1 = await _userManager.AddToRoleAsync(entityUser, role.Name);
                     

            //var code = _userManager.GenerateEmailConfirmationTokenAsync(entityUser).Result;

            List<string> err = new List<string>();
            foreach (var i in result.Errors)
            {
                err.Add(i.Description);
            }

            if (!result.Succeeded)
            {
                return new ApiResult<UserDto>(result: null, isSuccess: false, errors: err.ToArray());
            }
            entityUser.EmailConfirmed = true;            
            await  _userManager.UpdateAsync(entityUser);
            var newuser = _mapper.Map<UserDto>(entityUser);            
            return new ApiResult<UserDto>(user, isSuccess: true, errors: null);
        }
        public async Task<ApiResult<string>> SendEmail(string Email, string callbackUrl)
        {
            await _emailSender.SendEmailConfirmationAsync(Email, callbackUrl);
            return new ApiResult<string>(result: "ok", isSuccess: true, errors: null);
        }
        public async Task<string> Confirm(string userId, string code)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? "ConfirmEmail" : "Error";
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
        public async Task<ApiResult<UserDto>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResult<UserDto>(result: null, isSuccess: false, errors: new[] { "Пользователь с таким Email не найден", "" });
            }
            return new ApiResult<UserDto>(result: _mapper.Map<UserDto>(user), isSuccess: true, errors: null);
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
            oldUser.FIO = user.FIO;
            await _userManager.UpdateAsync(oldUser);          

            //OkResult ok = new OkResult();
            return new OkResult();
        }
        public async Task<ApiResult<User>> GetUserRole(Guid id)
        {
            string UserId = Convert.ToString(id);
            var user = _userManager.FindByIdAsync(UserId).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            return new ApiResult<User>(result: user, isSuccess: true, errors: null);
        }
    }
}
