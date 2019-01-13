using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Contracts.DTO.Base;

namespace WebApi.Contracts.DTO
{
    public class CategorySubcategories
    {
        public Dictionary<int, string> Subcategories { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
