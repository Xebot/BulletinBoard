using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace WebMVC.Models
{
    public class AdvertViewModel
    {
        public AdvertDto advert { get; set; }
        public int commentsNumber { get; set; }
        public int addingCommentsNumber { get; set; }

        public Guid currentUserId { get; set; }
    }
}