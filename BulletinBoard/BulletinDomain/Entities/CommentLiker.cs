using System;
using System.Collections.Generic;
using System.Text;
using BulletinDomain.Entities.Base;

namespace BulletinDomain.Entities
{
    public class CommentLiker : EntityBase<int>
    {
        /// <summary>
        /// ID пользователя, чей комментарий
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// ID пользователя, чей комментарий
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// Дата публикации комментария
        /// </summary>
        public DateTime PublicationDate { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
