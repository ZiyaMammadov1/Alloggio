using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core_Layer.Entities
{
    public class OrderRooms
    {
        public int id { get; set; }
        public int OrderId { get; set; }
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckIn { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public Order Order { get; set; }
        public Room Room { get; set; }

        [Required]
        public int Adult { get; set; }

        [Required]
        public int Children { get; set; }

        [Required]
        public int Infant { get; set; }
        public bool IsDeleted { get; set; }
    }
}
