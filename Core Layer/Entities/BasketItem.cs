using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Layer.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int RoomId { get; set; }
        public int Count { get; set; }
        public DateTime CreatedAt { get; set; }
        public AppUser AppUser { get; set; }
        public Room Room { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut{ get; set; }
        public int Adults { get; set; }
        public int Childrens { get; set; }
        public int Infants { get; set; }
    }
}
