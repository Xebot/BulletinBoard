using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace WebMVC.Models
{
    public class FilteredAdvertsViewModel
    {
        public List<AdvertDto> filteredAds { get; set; }
        public int PageNumber { get; set; }
        public int felteredAdsNumber { get; set; }
        public int advertsPerPageNumber { get; set; }
        public string CategoryId { get; set; }
        public string searchedText { get; set; }
        public string RegionId { get; set; }
        public string searchOnlyInTitle { get; set; }

    }
}
