using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BulletinDomain.Entities.Base;

namespace BulletinDomain.Entities
{
    /// <summary>
    /// Объявление
    /// </summary>
    public class Advert : EntityBase<Guid>
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdvertNumber { get; set; }

        /// <summary>
        /// ID пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        public IQuarable Category { get; set; }

        /// <summary>
        /// Id категории
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// Id категории
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string AdvertText { get; set; }
                
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
        /// Путь к главной картинка
        /// </summary>
        public string PrimaryImageUrl { get; set; }

        /// <summary>
        /// Пути к дополнительным картинкам
        /// </summary>
        public string ImagesUrl { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Имя автора объявления
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public List<Comment> Comments { get; set; }
    }
}
