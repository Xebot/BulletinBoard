using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class RegionViewModel
    {
        /// <summary>
        /// ИД вышестоящего региона
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// Наименование региона
        /// </summary>
        public string Name { get; set; }
    }
}
