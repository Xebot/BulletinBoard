using System;
using System.Collections.Generic;
using BulletinDomain.Entities.Base;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BulletinDomain.Entities
{
    public class Comment : EntityBase<int>
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
        public int ParentId { get; set; }

        /// <summary>
        /// ИД объявления, к которому относится комментарий
        /// </summary>
        public virtual Advert Advert { get; set; }

        public virtual ICollection<CommentLiker> CommentLikers { get; set; }
    }
}
