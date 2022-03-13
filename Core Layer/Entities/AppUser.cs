using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Layer.Entities
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
        public string Phone { get; set; }
    }
}
