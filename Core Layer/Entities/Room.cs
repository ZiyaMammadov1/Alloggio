using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core_Layer.Entities
{
    public class Room
    {
        public int id { get; set; }

        [StringLength(maximumLength: 150)]
        public string Image { get; set; }

        [StringLength(maximumLength: 150)]
        public string PanoramaImage { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }

        [Required]
        public int Size { get; set; }

       
        [StringLength(maximumLength: 150)]
        public string GuestRange { get; set; }

        [StringLength(maximumLength: 600)]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int BedCount { get; set; }
        public List<RoomAmenities> RoomAmenities { get; set; }
        public List<Comment> UserComments { get; set; }
        public List<OrderRooms> OrderRooms { get; set; }

        [NotMapped]
        [Required]
        public IFormFile MainPhoto { get; set; }


        [NotMapped]
        [Required]
        public IFormFile PanoramaPhoto { get; set; }



    }
}
