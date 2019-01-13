using AppServices.Interfaces;
using Authentication.AppServices.CookieAuthentication;
using Authenticaton.Contracts.Basic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;
using AppServices.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Identity;
using BulletinDomain.Entities;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Microsoft.Extensions.Localization;

namespace WebApiService.Controllers
{
    [ApiController, Route("api/[controller]"), Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtBasedCookieAuthenricationService _authenticationService;
        private readonly IAdminService _adminService;
        private readonly SignInManager<User> _signInManager;
        private readonly IStringLocalizer<UserController> _localizer;


        public UserController(IUserService userService, IJwtBasedCookieAuthenricationService authenticationService, IAdminService adminService, SignInManager<User> signInManager, IStringLocalizer<UserController> localizer)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _adminService = adminService;
            _signInManager = signInManager;
            _localizer = localizer;
        }

        [HttpPost]
        [Route("SignInAsync")]
        public async Task<ActionResult> SignInAsync([FromBody] BasicAuthenticationRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResult<UserDto>(result: null, isSuccess: false, errors: new string[] { _localizer["emptyRequest"] }));
            }
            
            var authenticationResult = await _authenticationService.SignInAsync(request);
            if (authenticationResult.IsSucceed)
            {
                return Ok(new ApiResult<UserDto>(result: null, isSuccess: true, errors: null));
            }
            else
            {
                return BadRequest(new ApiResult<UserDto>(result: null, isSuccess: false, errors: authenticationResult.Errors));
            }
        }

        [HttpPost]
        [Route("ExternalLogin")]
        public IActionResult ExternalLogin(string returnUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Microsoft", returnUrl);
            return Challenge(properties, "Microsoft");         
        }              

        [HttpPost]
        [AllowAnonymous]
        [Route("VkLogin")]
        public async Task<IActionResult> VkLogin([FromBody]TokenDto token)
        {           
                //смотрим был ли такой пользователь уже зарегистрирован?
                var info = await _userService.GetUserByEmailAsync(token.email);
                //если такого небыло и он впервые, то регистрируем его
                if (info.IsSuccess == false)
                {
                    var user = await _userService.CreateUserExternalAsync(token);
                    if (user.IsSuccess)
                    {
                        return Ok(new ApiResult<UserDto>(result: user.Result, isSuccess: true, errors: null));
                    }
                    else
                    {
                        return BadRequest(new ApiResult<UserDto>(result: null, isSuccess: false, errors: user.Errors));
                    }
                }
                else
                {                   
                   return Ok(new ApiResult<UserDto>(result: info.Result, isSuccess: true, errors: null));                     
                }                       
        }       
        
        [HttpPost,Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("MakeAdmin/{id}")]
        public async Task<ApiResult<UserDto>> MakeAdmin (Guid id)
        {
            var result = _adminService.MakeAdmin(id);            
            return result;
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("UnMakeAdmin/{id}")]
        public async Task<ApiResult<UserDto>> UnMakeAdmin(Guid id)
        {
            var result = _adminService.UnMakeAdmin(id);
            return result;
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("BanUser/{id}")]
        public async Task<ApiResult<UserDto>> BanUser(Guid id)
        {
            var result = await _adminService.banUser(id);
            return result;
        }
        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [Route("UnBanUser/{id}")]
        public async Task<ApiResult<UserDto>> UnBanUser(Guid id)
        {
            var result = await _adminService.banUser(id);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user.IsSuccess == false)
            {
                return BadRequest(user);
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("adduser")]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUser)
        {
            if (createUser == null)
            {
                return BadRequest();
            }
            var response = await _userService.CreateUserAsync(createUser);           

            if (response.IsSuccess == true)
            {
                //отправляем письмо с ссылкой для подтверждения e-mail
                var code = response.Errors[0];
                string Id = Convert.ToString(response.Result.Id);
                var callbackUrl = Url.EmailConfirmationLink(Id, code);
                await _userService.SendEmail(response.Result.UserEmail, callbackUrl);
                return Ok(response.Result);
            }
            else
            {
                return BadRequest(response);
            }
        }
       

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("updateUser")]
        public async Task<ActionResult> UpdateUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            await _userService.UpdateUserAsync(user);
            return Ok();

        }

        [HttpPost]
        [Route("getroles/{id}")]
        public async void GetRoles(Guid id)
        {
            await _userService.GetUserRole(id);
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
        [Route("GetAllUsers")]
        public List<UserDto> GetAllUsers()
        {
            var users = _adminService.GetAllUsersAsync();
            return users;
            
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                throw new ArgumentNullException();
            }
            var result = await _userService.Confirm(userId, code);
            if (result == "ConfirmEmail")
            {
                return Ok(_localizer["emailConfirmed"]);
            }
            else
            {
                return BadRequest(_localizer["emailNotConfirmed"]);
            }
            
        }

    }
}