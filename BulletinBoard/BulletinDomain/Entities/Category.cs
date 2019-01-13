using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BulletinDomain.Entities.Base;

namespace BulletinDomain.Entities
{
    public class IQuarable : EntityBase<int>
    {
        /// <summary>
        /// Наименование категории
        /// </summary>
        public string RuCategoryName { get; set; }

        public string EnCategoryName { get; set; }

        /// <summary>
        /// Url категории
        /// </summary>
        public string CategoryUrl { get; set; }

        /// <summary>
        /// Наименование категории
        /// </summary>
        public int ParentId { get; set; }

        public virtual ICollection<Advert> Adverts { get; set; }
    }
}
