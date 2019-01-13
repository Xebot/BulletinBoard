using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppServices.Interfaces;
using WebApi.Contracts.DTO;
using WebMVC.Models;
using System.Net.Http;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Authentication.AppServices.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdvertService _advertService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;


        public AdminController(IAdvertService advertService, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _advertService = advertService;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
            _client = new HttpClient();
        }

        [HttpGet]
        public async Task<ActionResult> Index(int pageNumber = 1)
        {
            UserAdvertsDto adverts = null;
            UserAdvertsViewModel userAdverts = new UserAdvertsViewModel();
            var url = string.Format(GetAbsolutePath("adverts", "AllAdmin"),pageNumber);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var response = await _client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    adverts = response.Content.ReadAsAsync<UserAdvertsDto>().Result;
                    userAdverts.AdsNumber = adverts.count;
                    userAdverts.Ads = adverts.ads;
                    userAdverts.advertsPerPageNumber = 12;
                    return View(userAdverts);
                }
            }
            return RedirectToAction("Index", "Home");
        }        

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            List<UserDto> users = null;

            var url = GetAbsolutePath("user", "GetAllUsers");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var response = await _client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    users = response.Content.ReadAsAsync<List<UserDto>>().Result;                    
                    Dictionary<int, string> regions = null;                    
                    var local = @System.Globalization.CultureInfo.CurrentCulture.Name;
                    url = string.Format(GetAbsolutePath("regions", "GetRegions"), local);
                    var response1 = _client.GetAsync(url).Result;
                    if (response1.IsSuccessStatusCode)
                    {
                            regions = response1.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                    }                   
                    
                    foreach (var t in users)
                    {
                        t.UserRegion = regions.GetValueOrDefault(t.RegionId);
                    }
                    return View(users);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Unpublish(Guid id)
        {
            var url = string.Format(GetAbsolutePath("adverts","Unpublish"),id);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            
            using (var response = await _client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var url = string.Format(GetAbsolutePath("adverts", "GetAdvert"), id);
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var response = await _client.SendAsync(request)) { }            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteTotal(Guid id)
        {
            var url = string.Format(GetAbsolutePath("adverts","total"), id);
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var httpClient = await _client.SendAsync(request))
            {
            }   
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> MakeUserAdmin(Guid id)
        {
            var url = string.Format(GetAbsolutePath("user","MakeAdmin"), id);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var response = await _client.SendAsync(request))
            {                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllUsers");
                }
                else
                {
                    ApiResult<UserDto> error = await response.Content.ReadAsAsync<ApiResult<UserDto>>();
                    ViewBag.Errors = error.Errors;
                    return View();
                }
            }            
        }

        public async Task<ActionResult> BanUser(Guid id)
        {
            var url = string.Format(GetAbsolutePath("user", "BanUser"), id);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var response = await _client.SendAsync(request))
            {               
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllUsers");
                }
                else
                {
                    ApiResult<UserDto> error = await response.Content.ReadAsAsync<ApiResult<UserDto>>();
                    ViewBag.Errors = error.Errors;
                    return View();
                }
            }
        }

        public async Task<ActionResult> UnMakeUserAdmin(Guid id)
        {
            var url = string.Format(GetAbsolutePath("user", "UnMakeAdmin"), id);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var response = await _client.SendAsync(request))
            {                
                if (response.IsSuccessStatusCode)
                {
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("GetAllUsers");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("GetAllUsers");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<ActionResult> UserInfo()
        {
            var context = _contextAccessor.HttpContext;            
            UserDto user = null;
            UserInfoViewModel viewUser;            
            Guid id = (Guid)context.User.GetUserId();
            ApiResult<UserDto> answer = null;
            var url = string.Format(GetAbsolutePath("user","GetUser"), id);
            var request = new HttpRequestMessage(HttpMethod.Get, url);            
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");

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
                    UserName = user.UserName,
                    UserAdress = user.UserAdress,
                    RegionId = user.RegionId,
                    Id = user.Id
                };
                // Получение словаря регионов (Id, Name)   
                var local = @System.Globalization.CultureInfo.CurrentCulture.Name;
                url = string.Format(GetAbsolutePath("regions", "GetRegions"), local);
                var response1 = _client.GetAsync(url).Result;
                if (response1.IsSuccessStatusCode)
                {
                    viewUser.Regions = response1.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                }
            }
            return View(viewUser);
        }

        [HttpPost]
        public async Task<ActionResult> UserInfo(UserInfoViewModel userInfo)
        {
            UserDto request = new UserDto
            {
                Id = userInfo.Id,
                UserName = userInfo.UserName,
                UserEmail = userInfo.Email,
                UserAdress = userInfo.UserAdress,
                RegionId = userInfo.RegionId,
                UserTel = userInfo.Phone
            };
            var url = GetAbsolutePath("user","updateuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
            using (var response = await _client.PostAsJsonAsync<UserDto>(url, request))
            {  
              
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