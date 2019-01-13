
using Microsoft.AspNetCore.Identity;
using System;


namespace BulletinDomain.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
    }
}
