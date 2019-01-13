using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace WebMVC.Models
{
    public class CategoryAdvertsViewModel
    {
        public string СategoryName { get; set; }
        public string СategoryUrl { get; set; }
        public int PageNumber { get; set; }
        public int categoryAdvertsNumber { get; set; }
        public int advertsPerPageNumber { get; set; }
        public Dictionary<string, string> SubcategoriesNameAndUrlDictionary { get; set; }
        public List<AdvertDto> AdvertsList { get; set; }
    }
}
