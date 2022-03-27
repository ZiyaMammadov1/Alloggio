using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core_Layer.Entities
{
    public class AppUser : IdentityUser
    {
        [StringLength(maximumLength:50)]
        public string Fullname { get; set; }

        [StringLength(maximumLength: 100)]
        public string Phone { get; set; }

        [StringLength(maximumLength: 150)]
        public string Image { get; set; }
        public bool IsAdmin { get; set; }
        public List<Comment> UserComments { get; set; }
    }
}
