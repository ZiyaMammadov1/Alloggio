using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core_Layer.Entities
{
    public class Testimonial
    {
        public int id { get; set; }
        [StringLength(maximumLength:50)]
        public string Name { get; set; }

        [StringLength(maximumLength: 350)]
        public string Description { get; set; }

        [StringLength(maximumLength: 150)]
        public string Image { get; set; }
    }
}
