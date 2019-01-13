using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApi.Contracts.DTO;

namespace WebMVC.Models
{
    /// <summary>
    /// Объявление
    /// </summary>
    public class EditAdvertModel
    {
        /// <summary>
        /// ID объявления
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id категории
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Id региона
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Статус объявления
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Дата создания объявления
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Заголовок объявления
        /// </summary>
        //[Display(Name = "Название")]
        [Required(ErrorMessage = "requireField")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "titleLength")]
        public string Title { get; set; }

        /// <summary>
        /// Картинка
        /// </summary>
        public IFormFile AdvertPrimaryImage { get; set; }

        /// <summary>
        /// Дополнительные изображения
        /// </summary>
        public List<IFormFile> AdvertImages { get; set; }

        /// <summary>
        /// Дополнительные изображения
        /// </summary>
        public string DeletedAdvertImages { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        [Required(ErrorMessage = "requireField")]
        [StringLength(400, MinimumLength = 20, ErrorMessage = "shortDescriptionLength")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        [Required(ErrorMessage = "requireField")]
        [StringLength(10000, MinimumLength = 50, ErrorMessage = "descriptionLength")]
        public string Description { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        [Required(ErrorMessage = "requireField")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "addressLength")]
        public string Address { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [Required(ErrorMessage = "requireField")]
        [RegularExpression("^[0-9]+[,.][0-9]{2}$", ErrorMessage = "priceFormat")]
        public string Price { get; set; }
    }
}
