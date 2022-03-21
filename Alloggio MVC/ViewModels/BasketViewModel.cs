using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.ViewModels
{
    public class BasketViewModel
    {
        public List<RoomBasketViewModel> BasketItems { get; set; }
        public double TotalPrice { get; set; }
    }
}
