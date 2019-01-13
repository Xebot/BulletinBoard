using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO.Base;

namespace WebApi.Contracts.DTO
{
    public class CommentLikerDto : EntityDto<int>
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
    }
}
