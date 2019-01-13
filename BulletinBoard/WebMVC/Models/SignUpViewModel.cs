using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "requireField")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "userNameLength")]
        public string FIO { get; set; }

        [Required(ErrorMessage = "requireField")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "passwordLength")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$", ErrorMessage = "passwordFormat")]
        public string Password { get; set; }

        [Required(ErrorMessage = "requireField")]
        [RegularExpression(@"\S+@\S+\.\S+", ErrorMessage = "emailFormat")]
        public string Email { get; set; }

        [Required(ErrorMessage = "requireField")]
        [RegularExpression(@"^\+[0-9]{1}\([0-9]{3}\) [0-9]{3}\-[0-9]{2}\-[0-9]{2}$", ErrorMessage = "phoneFormat")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "requireField")]
        [StringLength(180, MinimumLength = 5, ErrorMessage = "userAdressLength")]
        public string UserAdress { get; set; }
        
        public int RegionId { get; set; }
    }
}
