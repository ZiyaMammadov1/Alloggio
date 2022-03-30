using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core_Layer.Entities
{
    public class Slider
    {
        public int id { get; set; }

        [StringLength(maximumLength:150)]
        public string Image { get; set; }

        [StringLength(maximumLength: 50)]
        
        public string Header { get; set; }

        [NotMapped]
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
