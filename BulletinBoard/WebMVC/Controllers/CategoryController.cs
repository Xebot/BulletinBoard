using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using System.Net.Http;
using WebApi.Contracts.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using AppServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Localization;


namespace WebMVC.Controllers
{
    public class CategoryController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAdvertService _advertService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public CategoryController(IHostingEnvironment hostingEnvironment, IAdvertService advertService, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _advertService = advertService;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }
        
        public ActionResult CategoryPage(string categoryUrl, int pageNumber = 1, int advertsPerPageNumber = 12)
        {
            CategoryAdvertsViewModel viewModel = new CategoryAdvertsViewModel();
            viewModel.СategoryName = GetCategoryName(categoryUrl);
            viewModel.СategoryUrl = categoryUrl;
            viewModel.SubcategoriesNameAndUrlDictionary = GetSubcategoriesNameAndUrlDictionary(categoryUrl);
            viewModel.PageNumber = pageNumber;
            var local = @System.Globalization.CultureInfo.CurrentCulture.Name;                          
            var url = string.Format(GetAbsolutePath("categories", "get-category-adverts-number"),categoryUrl);            
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    viewModel.categoryAdvertsNumber = response.Content.ReadAsAsync<int>().Result;
                }
            }  
            viewModel.advertsPerPageNumber = advertsPerPageNumber;
            viewModel.AdvertsList = GetCategoryAdverts(categoryUrl, pageNumber, advertsPerPageNumber);
            return View(viewModel);
        }

        public string GetCategoryName(string categoryUrl)
        {
            string categoryName = "";
            var culture = @System.Globalization.CultureInfo.CurrentCulture.Name;
            var url = GetAbsolutePath("categories", "get-category-name-by-url" );
            var a = url + culture + categoryUrl;
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url + categoryUrl + "/" + culture).Result;
                if (response.IsSuccessStatusCode)
                {
                    categoryName = response.Content.ReadAsStringAsync().Result;
                }
            }
            return categoryName;
        }

        public Dictionary<string, string> GetSubcategoriesNameAndUrlDictionary(string categoryUrl)
        {
            Dictionary<string, string> subcategoriesNameAndUrlDictionary = new Dictionary<string, string>();
            var url = GetAbsolutePath("categories", "get-subcategories-name-and-url-dictionary");
            string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url + categoryUrl + "/" + cultureName).Result;
                if (response.IsSuccessStatusCode)
                {
                    subcategoriesNameAndUrlDictionary = response.Content.ReadAsAsync<Dictionary<string, string>>().Result;
                }
            }
            return subcategoriesNameAndUrlDictionary;
        }

        public List<AdvertDto> GetCategoryAdverts(string categoryUrl, int pageNumber, int advertsPerPageNumber)
        {
            List<AdvertDto> categoryAdverts = null;
            var url = GetAbsolutePath("categories", "get-category-adverts");
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url + categoryUrl + "/" + pageNumber.ToString() + "/" + advertsPerPageNumber.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    categoryAdverts = response.Content.ReadAsAsync<List<AdvertDto>>().Result;
                }
            }
            return categoryAdverts;
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
