using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Contracts.DTO
{
    public class UserAdvertsDto
    {
        public int count { get; set; }
        public List<AdvertDto> ads { get; set; }
    }
}
