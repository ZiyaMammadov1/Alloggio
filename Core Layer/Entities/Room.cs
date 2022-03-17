using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Layer.Entities
{
    public class Room
    {
        public int id { get; set; }
        public string Image { get; set; }
        public string PanoramaImage { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string GuestRange { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int BedCount { get; set; }
        public List<RoomAmenities> RoomAmenities { get; set; }
        public List<Comment> UserComments { get; set; }
        public List<OrderRooms> OrderRooms { get; set; }



    }
}
