using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core_Layer.Entities
{
    public class Blog
    {
        public int id { get; set; }


        [StringLength(maximumLength: 150)]
        public string Image { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string Header { get; set; }

        [Required]
        [StringLength(maximumLength: 1500)]
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
