using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace WebMVC.Models
{
    public class AddAdvertViewModel
    {
        public int RegionId { get; set; }

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "requireField")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "titleLength")]
        public string Title { get; set; }        
        
        [Required(ErrorMessage = "requireField")]
        public IFormFile AdvertPrimaryImage { get; set; }

        [Required(ErrorMessage = "requireField")]
        [StringLength(400, MinimumLength = 20, ErrorMessage = "shortDescriptionLength")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "requireField")]
        [StringLength(10000, MinimumLength = 50, ErrorMessage = "descriptionLength")]
        public string Description { get; set; }

        [Required(ErrorMessage = "requireField")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "addressLength")]
        public string Address { get; set; }

        [Required(ErrorMessage = "requireField")]
        [RegularExpression("^[0-9]+[,.][0-9]{2}$", ErrorMessage = "priceFormat")]
        public string Price { get; set; }

        public bool isNotValidAdvert { get; set; }

        public Dictionary<int, string> Regions { get; set; }

        public List<CategorySubcategories> CategoriesListWithSubcategories { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
