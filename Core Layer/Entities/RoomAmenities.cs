using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Layer.Entities
{
    public class RoomAmenities
    {
        public int id { get; set; }
        public int Roomid { get; set; }
        public int Amenitieid { get; set; }
        public Room Room { get; set; }
        public Amenitie Amenitie { get; set; }
    }
}
