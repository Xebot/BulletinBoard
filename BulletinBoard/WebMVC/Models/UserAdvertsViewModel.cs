using System;
using System.Collections.Generic;
using WebApi.Contracts.DTO;

namespace WebMVC.Models
{
    public class UserAdvertsViewModel
    {
        public List<AdvertDto> Ads { get; set; }
        public int PageNumber { get; set; }
        public int AdsNumber { get; set; }
        public int advertsPerPageNumber { get; set; }
        public string UserId { get; set; }
    }
}
