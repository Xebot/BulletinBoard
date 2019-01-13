using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Contracts.DTO
{
    public class FilteredDto
    {
        public AdvertDto[] adverts { get; set; }
        public int TotalCount { get; set; }
    }
}
