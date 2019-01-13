using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO.Base;

namespace WebApi.Contracts.DTO
{
    public class CommentDto : EntityDto<int>
    {
        /// <summary>
        /// ID пользователя, чей комментарий
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Url аватара пользователя
        /// </summary>
        public string UserAvatarUrl { get; set; }

        /// <summary>
        /// Id родителя
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// Дата публикации комментария
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// ИД объявления, к которому относится комментарий
        /// </summary>
        public Guid AdvertId { get; set; }

        /// <summary>
        /// ИД объявления, к которому относится комментарий
        /// </summary>
        public int CommentLikersNumber { get; set; }

        /// <summary>
        /// ИД объявления, к которому относится комментарий
        /// </summary>
        public List<CommentDto> CommentReplies { get; set; } = null;

        public virtual ICollection<CommentLikerDto> CommentLikers { get; set; }
    }
}
