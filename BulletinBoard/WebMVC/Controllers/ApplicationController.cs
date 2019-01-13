using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Net.Http;
using WebApi.Contracts.DTO;

namespace WebMVC.Controllers
{
    public abstract class ApplicationController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response;
                string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                ViewBag.cultureName = cultureName;
                if (cultureName == "ru")
                {
                    //Получение списка категорий 
                    response = httpClient.GetAsync("http://localhost:58886/api/categories/categories-dictionary/ru").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.CategoriesListWithSubcategories = response.Content.ReadAsAsync<List<CategorySubcategories>>().Result;
                        ViewData["Category"] = response.Content.ReadAsAsync<List<CategorySubcategories>>().Result;
                    }
                    //Получение словаря регионов (Id, Name)         
                    response = httpClient.GetAsync("http://localhost:58886/api/regions/get-regions-dictionary/ru").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                    }
                }
                else
                {
                    //Получение списка категорий 
                    response = httpClient.GetAsync("http://localhost:58886/api/categories/categories-dictionary/en").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.CategoriesListWithSubcategories = response.Content.ReadAsAsync<List<CategorySubcategories>>().Result;
                        ViewData["Category"] = response.Content.ReadAsAsync<List<CategorySubcategories>>().Result;
                    }
                    //Получение словаря регионов (Id, Name)         
                    response = httpClient.GetAsync("http://localhost:58886/api/regions/get-regions-dictionary/en").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }


        //public ApplicationController()
        //{

        //}
    }
}
