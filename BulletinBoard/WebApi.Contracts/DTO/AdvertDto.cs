using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Contracts.DTO.Base;
namespace WebApi.Contracts.DTO
{
    /// <summary>
    /// Объявление
    /// </summary>
    public class AdvertDto : EntityDto<Guid>
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Номер объявления
        /// </summary>
        public int AdvertNumber { get; set; }

        /// <summary>
        /// Id категории
        /// </summary>
        public int CategoryId { get; set; }      

        /// <summary>
        /// Id категории
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string AdvertText { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public List<CommentDto> Comments { get; set; }

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
        public string Title { get; set; }

        /// <summary>
        /// Имя папки
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// Картинка
        /// </summary>
        public string PrimaryImageUrl { get; set; }

        /// <summary>
        /// Картинка
        /// </summary>
        public string ImagesUrl { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>        
        public string Email { get; set; }


        /// <summary>
        /// Короткое описание
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public string UserFIO { get; set; }
    } 
}
