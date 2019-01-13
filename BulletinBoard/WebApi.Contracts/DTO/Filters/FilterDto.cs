using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Contracts.DTO.Filters
{
    public class Range<T>
    {
        public T MinV { get; set; }
        public T MaxV { get; set; }
    }
    public class FilterDto
    {
        public string Title { get; set; }

        public Range<decimal?> PriceRange { get; set; }

        public string Region { get; set; }

        public string Category { get; set; }

        public bool Pic { get; set; }

        public string Text { get; set; }

        public string isActive { get; set; }

        public string Role { get; set; }

        public int PageNumber { get; set; }
        public int felteredAdsNumber { get; set; }
        public int advertsPerPageNumber { get; set; }
    }
}
