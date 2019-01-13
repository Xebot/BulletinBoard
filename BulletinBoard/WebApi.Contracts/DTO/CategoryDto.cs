using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO.Base;

namespace WebApi.Contracts.DTO
{
    public class CategoryDto : EntityDto<int>
    {
        /// <summary>
        /// Наименование категории
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Наименование категории
        /// </summary>
        public string CategoryUrl { get; set; }

        /// <summary>
        /// Наименование категории
        /// </summary>
        public int GroupId { get; set; }

        public virtual ICollection<AdvertDto> Adverts { get; set; }
    }
}
