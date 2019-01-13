using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppServices.Interfaces;
using WebApi.Contracts.DTO;
using WebMVC.Models;
using System.Net.Http;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Authentication.AppServices.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace WebUI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAdvertService _advertService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        private readonly HttpClient _client;

        public UserController(IHostingEnvironment hostingEnvironment, IAdvertService advertService, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _advertService = advertService;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
            _client = new HttpClient();

        }
        
        [HttpGet]
        public async Task<ActionResult> Index(string userId,int pageNumber = 1)
        {
            UserAdvertsDto adverts = null;
            UserAdvertsViewModel userAdverts = new UserAdvertsViewModel();
            var context = _contextAccessor.HttpContext;
            var token = context.User.GetAuthToken();
            Guid id = (Guid)context.User.GetUserId();
            var url = string.Format(GetAbsolutePath("adverts", "UserAdverts"),id, pageNumber);
            var request = new HttpRequestMessage(HttpMethod.Get, url);            
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            using (var response = await _client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    adverts = response.Content.ReadAsAsync<UserAdvertsDto>().Result;
                    userAdverts.Ads = adverts.ads;
                    userAdverts.AdsNumber = adverts.count;
                    userAdverts.advertsPerPageNumber = 12;
                    return View(userAdverts);
                }
            };            
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Unpublish(Guid id)
        {
            var url = string.Format(GetAbsolutePath("adverts", "Unpublish"), id);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var response = await _client.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Cannot Unpublish advert. See details in API/Adverts";
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var url = string.Format(GetAbsolutePath("adverts","GetAdvert"),id);
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var response = await _client.SendAsync(request))
            {                
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> UserInfo()
        {
            var context = _contextAccessor.HttpContext;
            var token = context.User.GetAuthToken();
            UserDto user = null;
            UserInfoViewModel viewUser;            
            Guid id = (Guid)context.User.GetUserId();
            ApiResult<UserDto> answer = null;
            var url = string.Format(GetAbsolutePath("user","GetUser"), id);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            using (var response = await _client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    answer = await response.Content.ReadAsAsync<ApiResult<UserDto>>();
                    user = answer.Result;
                }
                else
                {
                    ViewBag.Errors = answer.Errors;
                    return PartialView("_UserErrors");
                }

                viewUser = new UserInfoViewModel
                {
                    Email = user.UserEmail,
                    Phone = user.UserTel,
                    UserName = user.UserEmail,
                    UserAdress = user.UserAdress,
                    RegionId = user.RegionId,
                    Id = user.Id,
                    FIO = user.FIO
                };
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage regionsResponse;
                    string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                    //Получение словаря регионов (Id, Name) 
                    url = string.Format(GetAbsolutePath("Regions", "GetRegions"), cultureName);
                    regionsResponse = httpClient.GetAsync(url).Result;
                    if (regionsResponse.IsSuccessStatusCode)
                    {
                        viewUser.Regions = regionsResponse.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                    }               
                }
                viewUser.Region = viewUser.Regions.GetValueOrDefault(viewUser.RegionId);
                return View(viewUser);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> UserInfoById(Guid id)
        {
            UserDto user = null;
            UserInfoViewModel viewUser;          
            ApiResult<UserDto> answer = null;
            var url = string.Format(GetAbsolutePath("user", "GetUser"), id);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                answer = await response.Content.ReadAsAsync<ApiResult<UserDto>>();
                if (response.IsSuccessStatusCode)
                {                    
                    user = answer.Result;
                }
                else
                {
                    ViewBag.Errors = answer.Errors;
                    return PartialView("_UserErrors");
                }                
                viewUser = new UserInfoViewModel
                {
                    Email = user.UserEmail,
                    Phone = Convert.ToInt64(user.UserTel).ToString("+#(###)###-##-##"),
                    UserName = user.UserName,
                    UserAdress = user.UserAdress,
                    RegionId = user.RegionId,
                    Id = user.Id,
                    FIO = user.FIO
                };
                // Получение словаря регионов (Id, Name)  
                string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                url = string.Format(GetAbsolutePath("Regions", "GetRegions"), cultureName);
                response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    viewUser.Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                }
            }
            viewUser.Region = viewUser.Regions.GetValueOrDefault(viewUser.RegionId);
            return View(viewUser);
        }

        [HttpPost]
        public ActionResult UnvalidUserInfo(UserInfoViewModel userInfo)
        {
            using (var httpClient = new HttpClient())
            {
                string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                var url = string.Format(GetAbsolutePath("Regions", "GetRegions"), cultureName);
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    userInfo.Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                }                
            }
            //viewUser.Region = viewUser.Regions.GetValueOrDefault(viewUser.RegionId);
            return View(userInfo);
        }

        [HttpPost]
        public async Task<ActionResult> UserInfo(UserInfoViewModel userInfo)
        {
            if (!ModelState.IsValid)
            {
                return UnvalidUserInfo(userInfo);
            }
            var context = _contextAccessor.HttpContext;
            var token = context.User.GetAuthToken();
            if (token == null)
            {
                throw new ArgumentNullException();
            }                        
            UserDto request = new UserDto
            {
                Id = userInfo.Id,
                UserName = userInfo.Email,
                UserEmail = userInfo.Email,
                UserAdress = userInfo.UserAdress,
                RegionId = userInfo.RegionId,
                UserTel = userInfo.Phone,
                FIO = userInfo.FIO
            };
            var url = GetAbsolutePath("User", "updateuser");           
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<UserDto>(url, request);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        private string GetAbsolutePath(string controller, string methodName)
        {
            return $"{_configuration["ServiceApi:BaseUrl"]}{_configuration[$"ServiceApi:Areas:{controller}:{methodName}"]}";
        }
        private string GetToken()
        {
            var context = _contextAccessor.HttpContext;
            return context.User.GetAuthToken();
        }
    }
    
}