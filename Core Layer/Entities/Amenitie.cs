﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Layer.Entities
{
    public class Amenitie
    {
        public int id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public List<RoomAmenities> RoomAmenities { get; set; }

    }
}