using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Contracts.DTO;
using WebApi.Contracts.DTO.Filters;

namespace AppServices.Interfaces
{
    public interface ICategoryService
    {
        IList<CategoryDto> GetAll();
        int GetCategoryIdByUrl(string categoryUrl);
        string GetCategoryNameByUrl(string categoryUrl, string culture);
        List<AdvertDto> GetCategoryAdvertsByUrl(string categoryUrl, int pageNumber, int advertsPerPageNumber);
        List<CategorySubcategories> GetCategoriesListWithSubcategories(string culture);
        Dictionary<string, string> GetSubcategoriesNameAndUrlDictionary(string categoryUrl, string culture);
        int GetCategoryAdvertsNumber(string categoryUrl);


        List<CategoryDto> GetAllCategories();        
        List<string> GetSubcategoriesUrlListByCategoryUrl(string categoryUrl);
    }
}
