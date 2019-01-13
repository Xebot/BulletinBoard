using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using System.Net.Http;
using WebApi.Contracts.DTO;
using WebApi.Contracts.DTO.Filters;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using AppServices.Interfaces;
using Authentication.AppServices.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;

namespace WebMVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAdvertService _advertService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public HomeController(IHostingEnvironment hostingEnvironment, IAdvertService advertService, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _advertService = advertService;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            IList<AdvertDto> ads = GetLastTwelveAdverts();
            return View(ads);
        }

        public IList<AdvertDto> GetLastTwelveAdverts()
        {
            IList<AdvertDto> adverts = null;
            var url = GetAbsolutePath("adverts", "get-last-twelve-adverts");
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    adverts = response.Content.ReadAsAsync<IList<AdvertDto>>().Result;
                }
            }
            return adverts;
        }
        
        [HttpPost]
        public JsonResult Index1(string Prefix)
        {
            List<AdvertDto> titles = null;
            var url = GetAbsolutePath("adverts", "all");
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    titles = response.Content.ReadAsAsync<List<AdvertDto>>().Result;
                }
            }
            var items = from t in titles where t.Title.ToLower().Contains(Prefix.ToLower()) && t.Status=="Published" select new { t.Title };            
            var i = items.Distinct().Take(10).ToList();            
            return Json(i);
        }

        [HttpGet]
        public async Task<IActionResult> Find(string СategoryId, string searchedText, string RegionId,
                                              string searchOnlyInTitle = null, string advertsOnlyWithPhoto = null, int pageNumber = 1)
        {

            FilterDto filter = new FilterDto();
            FilteredAdvertsViewModel filtered = new FilteredAdvertsViewModel();
            FilteredDto filt = new FilteredDto();
            filter.advertsPerPageNumber = 12;
            filter.PageNumber = pageNumber;
            if (Convert.ToInt32(СategoryId) != 1)
            {
                filter.Category = СategoryId;
                filtered.CategoryId = СategoryId;
            }
            if (RegionId != "Любой регион")
            {
                filter.Region = RegionId;
                filtered.RegionId = RegionId;
            }                     
            if (User.IsInRole("Admin"))
            {
                filter.Role = "Admin";
            }
            else
            {
                filter.isActive = "Published";
            }
            if (searchOnlyInTitle == null)
            {
                filter.Text = searchedText;
                filtered.searchOnlyInTitle = null;
            }
            filter.Title = searchedText;
            filtered.searchedText = searchedText;            
            var url = GetAbsolutePath("adverts","filter");            
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync<FilterDto>(url, filter).Result;
                if (response.IsSuccessStatusCode)
                {
                    filt = await response.Content.ReadAsAsync<FilteredDto>();
                }
            }
            filtered.filteredAds = filt.adverts.ToList();
            filtered.felteredAdsNumber = filt.TotalCount;
            filtered.advertsPerPageNumber = 12;
            return View(filtered);
        }         
        public async Task<IList<AdvertDto>> GetAdverts()
        {
            IList<AdvertDto> adverts = null;
            var url = GetAbsolutePath("adverts", "all");
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    adverts = await response.Content.ReadAsAsync<IList<AdvertDto>>();
                }
            }
            return adverts;
        }
                
        public async Task<IActionResult> Delete(Guid id)
        {
            var context = _contextAccessor.HttpContext;
            var token = context.User.GetAuthToken();
            if (token == null)
            {
                throw new ArgumentNullException();
            }
            var url = string.Format(GetAbsolutePath("adverts", "GetAdvert"),id);
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Headers =
                    {
                        { HttpRequestHeader.ContentType.ToString(),"application/json" },
                        { HttpRequestHeader.Authorization.ToString(), $"Bearer {token}" }
                    }
            };
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }       

        public Dictionary<int, string> GetRegions()
        {
            Dictionary<int, string> Regions = null;
            var local = @System.Globalization.CultureInfo.CurrentCulture.Name;
            var url = string.Format(GetAbsolutePath("regions", "GetRegions"),local);
            using (var httpClient = new HttpClient())
            {
                //Получение словаря регионов (Id, Name)    
                
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                }
            }
            return Regions;
        }
        
        public List<CategorySubcategories> GetCategories()
        {
            List<CategorySubcategories> categoriesListWithSubcategories = null;
            var local = @System.Globalization.CultureInfo.CurrentCulture.Name;
            var url = string.Format(GetAbsolutePath("categories", "categories-dictionary"), local);
            using (var httpClient = new HttpClient())
            {
                //Получение списка категорий 
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {                    
                    categoriesListWithSubcategories = response.Content.ReadAsAsync<List<CategorySubcategories>>().Result;
                }                
            }
            return categoriesListWithSubcategories;
        }

        [HttpGet]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private string GetAbsolutePath(string controller, string methodName)
        {
            return $"{_configuration["ServiceApi:BaseUrl"]}{_configuration[$"ServiceApi:Areas:{controller}:{methodName}"]}";
        }
    }
}
