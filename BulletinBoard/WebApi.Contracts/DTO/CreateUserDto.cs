using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApi.Contracts.DTO
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string Password { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserAdress { get; set; }
        public int RegionId { get; set; }
    }
}
