using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.ViewModels
{
    public class MemberUpdateViewModel
    {
        public string CurrentMemberName { get; set; }

        public string Image { get; set; }

        [Required]
        [StringLength(maximumLength: 30)]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        public string Fullname { get; set; }

        [Required]
        [StringLength(maximumLength: 20)]
        public string Phone { get; set; }

        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }

        public IFormFile UploadImage { get; set; }

    }
}
