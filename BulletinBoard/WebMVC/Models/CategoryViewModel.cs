using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class CategoryViewModel
    {
        /// <summary>
        /// ИД вышестоящей категории
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// Наименование категории
        /// </summary>
        public string Name { get; set; }
    }
}
