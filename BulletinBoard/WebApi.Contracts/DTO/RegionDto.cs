using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebApi.Contracts.DTO.Base;

namespace WebApi.Contracts.DTO
{
    public class RegionDto : EntityDto<int>
    {
        /// <summary>
        /// Наименование региона
        /// </summary>
        public string Name { get; set; }
    }
}
