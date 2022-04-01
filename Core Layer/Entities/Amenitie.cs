using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core_Layer.Entities
{
    public class Amenitie
    {
        public int id { get; set; }

        [Required]
        [StringLength(maximumLength:100)]
        public string Image { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        public List<RoomAmenities> RoomAmenities { get; set; }

    }
}
