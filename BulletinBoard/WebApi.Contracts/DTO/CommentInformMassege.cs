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
    public class CommentInformMassege
    {
        /// <summary>
        /// Номер объявления
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string UserAction { get; set; }

        /// <summary>
        /// Номер объявления
        /// </summary>
        public string AdvertTitle { get; set; }

        /// <summary>
        /// Номер объявления
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Номер объявления
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// Номер объявления
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Номер объявления
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// Номер объявления
        /// </summary>
        public Guid AdvertId { get; set; }

        public bool IsUnreadMassege { get; set; }
        
    }
}
