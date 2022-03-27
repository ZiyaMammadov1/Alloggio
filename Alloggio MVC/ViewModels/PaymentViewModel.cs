using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.ViewModels
{
    public class PaymentViewModel
    {
        public Order Order{ get; set; }
        public string? CardNumber{ get; set; }
        public int ?ExpiredMonth { get; set; }
        public int? ExpiredYear { get; set; }
        public int ?CardCVC { get; set; }
    }
}
