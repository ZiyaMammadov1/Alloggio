using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core_Layer.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string AppUserId { get; set; }

        [StringLength(maximumLength:150)]
        public string Image { get; set; }

        [StringLength(maximumLength: 50)]
        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }


        [StringLength(maximumLength: 350)]
        [Required]
        public string Text { get; set; }
        public AppUser AppUser { get; set; }
        public Room Room{ get; set; }
    }
}
