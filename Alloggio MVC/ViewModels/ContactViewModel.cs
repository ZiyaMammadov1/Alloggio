using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.ViewModels
{
    public class ContactViewModel
    {
        public Dictionary<string, string> Setting { get; set; }
        public Message Message{ get; set; }
    }
}
