using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.ViewModels
{
    public class MemberRegisterViewModel
    {
        [Required]
        [StringLength(maximumLength:20)]
        public string Username { get; set; }

        [Required]
        [StringLength(maximumLength: 30)]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Fullname { get; set; }

        [Required]
        [StringLength(maximumLength: 20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
