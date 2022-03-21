using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.ViewModels
{
    public class BasketItemViewModel
    {
        public int Roomid { get; set; }
        public int Count { get; set; }
        public DateTime CheckIn{ get; set; }
        public DateTime CheckOut{ get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
    }
}
