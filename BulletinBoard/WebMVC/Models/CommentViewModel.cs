using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace WebMVC.Models
{
    public class CommentViewModel
    {
        /// <summary>
        /// ИД вышестоящей категории
        /// </summary>        
        public int Id { get; set; }

        /// <summary>
        /// ID пользователя, чей комментарий
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        //public int LikesNumber { get; set; }

        /// <summary>
        /// ID родителя
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Путь к аватару
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// ИД объявления, к которому относится комментарий
        /// </summary>
        public Guid AdvertId { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// Дата и время публикации комментария
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public string LocalDate { get; set; }

        /// <summary>
        /// Время
        /// </summary>
        public string LocalTime { get; set; }

        /// <summary>
        /// ИД объявления, к которому относится комментарий
        /// </summary>
        public int CommentLikersNumber { get; set; }

        /// <summary>
        /// ИД объявления, к которому относится комментарий
        /// </summary>
        public bool IsCurrentUserComment { get; set; }

        /// <summary>
        /// ИД объявления, к которому относится комментарий
        /// </summary>
        public bool IsAuthorizedCurrentUser { get; set; }

        /// <summary>
        /// ИД объявления, к которому относится комментарий
        /// </summary>
        public bool IsLikedByUser { get; set; }

        public bool IsLastComment { get; set; }
               
        //public ICollection<CommentLikerDto> CommentLikers { get; set; }

        public List<CommentViewModel> CommentReplies { get; set; } = null;
    }
}
