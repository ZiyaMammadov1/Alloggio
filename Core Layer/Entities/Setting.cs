using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core_Layer.Entities
{
    public class Setting
    {
        public int id { get; set; }

       
        [StringLength(maximumLength:150)]
        public string Key { get; set; }

        [Required]
        [StringLength(maximumLength: 500)]
        public string Value { get; set; }

        [NotMapped]
        public IFormFile Photo{ get; set; }
    }
}
