using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.DTO;
using WebMVC.Models;
using System.Net.Http;
using Authenticaton.Contracts.Basic;
using Authentication.AppServices.CookieAuthentication;
using RestSharp;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace WebUI.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IJwtBasedCookieAuthenricationService _authenticationService;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<UserController> _localizer;

        public AuthorizationController(IJwtBasedCookieAuthenricationService authenticationService, IConfiguration configuration, IStringLocalizer<UserController> localizer)
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
            _localizer = localizer;
        }
        

        [HttpGet]
        public IActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public async Task<ActionResult> ExternalSignIn(string code = null, string email = null)
        {
            if (code != null)
            {
                TokenDto token = null;                
                UserDto user = null;
                var url = string.Format(GetAbsolutePathExternal("Vk","url"), code);
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        token = response.Content.ReadAsAsync<TokenDto>().Result;
                        url = string.Format(GetAbsolutePathExternal("vk","user"),token.user_id,token.access_token);
                        response = await httpClient.GetAsync(url);                       
                        var usr1 = response.Content.ReadAsStringAsync().Result.Split("\"");                        
                        token.userName = string.Concat(usr1[7], " ", usr1[11]);                                                
                    }
                    url = GetAbsolutePathExternal("vk", "Login");                    
                    response = await httpClient.PostAsJsonAsync<TokenDto>(url, token);
                    if (response.IsSuccessStatusCode)
                    {
                        var answer = response.Content.ReadAsAsync<ApiResult<UserDto>>().Result;
                        user = answer.Result;
                        var authenticationResult = await _authenticationService.SignInExternalAsync(user.UserEmail);
                        if (!authenticationResult.IsSucceed)
                        {
                            ViewBag.Errors = authenticationResult.Errors;
                            ViewBag.Pic = "~/images/401.png";
                            return View();
                        }
                        return RedirectToAction("UserInfo", "User");
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            else { return Unauthorized(_localizer["tokenErrorVK"]); }                          

        }        
        public async Task<ActionResult> ExternalSignInGoogle(string code = null, string email = null)
        {
            if (code != null)
            {                             
                UserDto user = null;
                var url = string.Format(GetAbsolutePathExternal("Google", "url"), code);
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                IRestResponse response1 = client.Execute(request);
                                
                string[] mas = response1.Content.Split("\"");                
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(mas[17]) as JwtSecurityToken;

                var user_email = tokenS.Claims.FirstOrDefault(claim => claim.Type == "email").Value;
                var user_name = tokenS.Claims.FirstOrDefault(claim => claim.Type == "name").Value;
                TokenDto token = new TokenDto { email = user_email, userName = user_name };                
                using (var httpClient = new HttpClient())
                {
                    url = GetAbsolutePathExternal("vk", "Login");
                    var response = await httpClient.PostAsJsonAsync<TokenDto>(url, token);
                    var answer = response.Content.ReadAsAsync<ApiResult<UserDto>>().Result;
                    user = answer.Result;
                    var authenticationResult = await _authenticationService.SignInExternalAsync(user.UserEmail);
                    if (!authenticationResult.IsSucceed)
                    {
                        ViewBag.Errors = authenticationResult.Errors;
                        ViewBag.Pic = "~/images/401.png";
                        return View();
                    }
                    return RedirectToAction("UserInfo", "User");
                }                
            }
            else { return Unauthorized(_localizer["tokenErrorMS"]); }

        }
        public async Task<ActionResult> ExternalSignInMS(string code = null, string email = null)
        {
            if (code != null)
            {

                var url = string.Format(GetAbsolutePathExternal("MS", "POST"), code);
                UserDto user = null;                
                var client = new RestClient("https://login.microsoftonline.com/common/oauth2/v2.0/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "5f9e9fc9-60c9-4ed3-a46f-9188b1bcec03");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("undefined", url, ParameterType.RequestBody);
                                
                IRestResponse response1 = client.Execute(request);

                string[] mas = response1.Content.Split("\"");
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(mas[19]) as JwtSecurityToken;

                var user_email = tokenS.Claims.FirstOrDefault(claim => claim.Type == "email").Value;
                var user_name = tokenS.Claims.FirstOrDefault(claim => claim.Type == "name").Value;
                TokenDto token = new TokenDto { email = user_email, userName = user_name };

                using (var httpClient = new HttpClient())
                {
                    url = GetAbsolutePathExternal("vk", "Login");
                    var response = await httpClient.PostAsJsonAsync<TokenDto>(url, token);
                    var answer = response.Content.ReadAsAsync<ApiResult<UserDto>>().Result;
                    user = answer.Result;
                    var authenticationResult = await _authenticationService.SignInExternalAsync(user.UserEmail);
                    if (!authenticationResult.IsSucceed)
                    {
                        ViewBag.Errors = authenticationResult.Errors;
                        ViewBag.Pic = "~/images/401.png";
                        return View();
                    }
                    return RedirectToAction("UserInfo", "User");
                }
            }
            else { return Unauthorized(_localizer["tokenErrorMS"]); }

        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInRequest, string returnUrl)
        {

            if (signInRequest == null)
            {
                return BadRequest();
            }
            
            var authenticationResult = await _authenticationService.SignInAsync(new BasicAuthenticationRequest
            {
                Password = signInRequest.Password,
                Email = signInRequest.Email
            });           
            if (!authenticationResult.IsSucceed)
            {                
                ViewBag.Errors = authenticationResult.Errors;
                ViewBag.Pic = "~/images/401.png";
                return View();
            }
            if (returnUrl == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return LocalRedirect(returnUrl);
            }        
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            Dictionary<int, string> Regions = null;            
            var local = @System.Globalization.CultureInfo.CurrentCulture.Name;
            var url = string.Format(GetAbsolutePath("regions", "GetRegions"), local);
            using (var httpClient = new HttpClient())
            {
                //Получение словаря регионов (Id, Name)         
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                }
            }
            ViewBag.Regions = Regions;
            return View();            
        }

        [HttpPost]
        public IActionResult UnvalidSignUp(SignUpViewModel signUpRequest)
        {
            Dictionary<int, string> Regions = null;
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response;
                string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                ViewBag.cultureName = cultureName;
                var url = string.Format(GetAbsolutePath("regions", "GetRegions"), cultureName);
                
                    //Получение словаря регионов (Id, Name)         
                    response = httpClient.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                    }              
                
            }
            return View(signUpRequest);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpRequest)
        {
            if (!ModelState.IsValid)
            {
                return UnvalidSignUp(signUpRequest);
            }
            var createUser = new CreateUserDto
            {
                UserName = signUpRequest.Email,
                Password = signUpRequest.Password,
                Email = signUpRequest.Email,
                Phone = signUpRequest.Phone,
                UserAdress = signUpRequest.UserAdress,
                RegionId = signUpRequest.RegionId,
                FIO = signUpRequest.FIO                
            };
            var url = GetAbsolutePath("user","add");
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<CreateUserDto>(url, createUser);
                if (response.IsSuccessStatusCode)
                {
                    var newUser = await response.Content.ReadAsAsync<ApiResult<UserDto>>();

                    ViewBag.Good = _localizer["mailSend"];
                    //ViewBag.Good = "Учетная запись создана. На e-mail отправлена ссылка для подтверждения e-mail";
                    //return View();
                    TempData["Message"] = "Confirm your e-mail";                    
                    return RedirectToAction("SignIn", "Authorization");
                }
                else
                {
                    ApiResult<UserDto> error = await response.Content.ReadAsAsync<ApiResult<UserDto>>();
                    ViewBag.Errors = error.Errors;
                    return View();
                }
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _authenticationService.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        private string GetAbsolutePath(string controller, string methodName)
        {
            return $"{_configuration["ServiceApi:BaseUrl"]}{_configuration[$"ServiceApi:Areas:{controller}:{methodName}"]}";
            //return $"{_configuration[$"ExternalProvider:{controller}:{methodName}"]}";
        }
        private string GetAbsolutePathExternal(string controller, string methodName)
        {
            //return $"{_configuration["ServiceApi:BaseUrl"]}{_configuration[$"ServiceApi:Areas:{controller}:{methodName}"]}";
            return $"{_configuration[$"ExternalProvider:{controller}:{methodName}"]}";
        }

    }
}