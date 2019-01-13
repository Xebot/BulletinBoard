using System;
using System.Collections.Generic;
using System.Text;
using BulletinDomain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinDomain.Entities
{
    public class Region : EntityBase<int>
    {
        /// <summary>
        /// Наименование региона
        /// </summary>
        public string RuName { get; set; }

        public string EnName { get; set; }

        public virtual ICollection<Advert> Adverts { get; set; }
    }
}
