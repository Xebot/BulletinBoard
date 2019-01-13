using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.DTO;
using AppServices.Interfaces;

namespace WebApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController(ICategoryService _categoryService)
        {
            categoryService = _categoryService;
        }
        protected readonly ICategoryService categoryService;

        // GET api/values
        [HttpGet("all")]
        public ActionResult<List<CategoryDto>> GetAll()
        {
            IList<CategoryDto> categories = categoryService.GetAll();
            return Ok(categories);
        }

        // GET api/values
        [HttpGet("categories-dictionary/{culture}")]
        public ActionResult<List<CategorySubcategories>> GetCategoriesListWithSubcategories(string culture = "en")
        {
            List<CategorySubcategories> categoriesListWithSubcategories = categoryService.GetCategoriesListWithSubcategories(culture);
            return Ok(categoriesListWithSubcategories);
        }

        // GET api/values
        [HttpGet("get-category-name-by-url/{categoryUrl}/{culture}")]
        public ActionResult<string> GetCategoryName(string categoryUrl, string culture = "en")
        {
            string categoryName = categoryService.GetCategoryNameByUrl(categoryUrl, culture);
            return Ok(categoryName);
        }

        // GET api/values
        [HttpGet("get-category-adverts/{categoryUrl}/{pageNumber}/{advertsPerPageNumber}")]
        public ActionResult<List<AdvertDto>> GetCategoryAdverts(string categoryUrl, int pageNumber, int advertsPerPageNumber)
        {
            List<AdvertDto> categoryAdverts = categoryService.GetCategoryAdvertsByUrl(categoryUrl, pageNumber, advertsPerPageNumber);
            return Ok(categoryAdverts);
        }

        [HttpGet("get-category-adverts-number/{categoryUrl}")]
        public ActionResult<IEnumerable<string>> GetCategoryAdvertsNumber(string categoryUrl)
        {
            int categoryAdvertsNumber = categoryService.GetCategoryAdvertsNumber(categoryUrl);
            return Ok(categoryAdvertsNumber);
        }

        // GET api/values
        [HttpGet("get-subcategories-name-and-url-dictionary/{categoryUrl}/{culture}")]
        public ActionResult<Dictionary<string, string>> GetSubcategoriesNameAndUrlDictionary(string categoryUrl, string culture = "en")
        {
            Dictionary<string, string> subcategoriesNameAndUrlDictionary = categoryService.GetSubcategoriesNameAndUrlDictionary(categoryUrl, culture);
            return Ok(subcategoriesNameAndUrlDictionary);
        }
        
    }
}
