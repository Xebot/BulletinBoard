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
    public class AdvertModel
    {
        /// <summary>
        /// ID объявления
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Номер объявления
        /// </summary>
        public int AdvertNumber { get; set; }

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
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Длина строки должна быть от 5 до 80 символов")]
        public string Title { get; set; }

        /// <summary>
        /// Картинка
        /// </summary>
        [Required(ErrorMessage = "Укажите главное изображение!")]
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
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [StringLength(400, MinimumLength = 20, ErrorMessage = "Длина строки должна быть от 20 до 400 символов")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [StringLength(10000, MinimumLength = 50, ErrorMessage = "Длина строки должна быть от 50 до 10 000 символов")]
        public string Description { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Длина строки должна быть от 10 до 200 символов")]
        public string Address { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [Required(ErrorMessage = "Поле должно быть заполнено!")]
        [RegularExpression("^[0-9]+[,][0-9]{2}$", ErrorMessage = "Не верный формат цены (X,XX)")]
        public decimal Price { get; set; }
        public Guid UserId { get; set; }
    }
}
