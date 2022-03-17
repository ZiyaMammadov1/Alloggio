using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Layer.Entities
{
    public class Checking
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
    }
}
