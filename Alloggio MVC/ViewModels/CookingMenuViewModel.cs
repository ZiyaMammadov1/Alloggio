using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.ViewModels
{
    public class CookingMenuViewModel
    {
        public List<CookingMenus> Menus { get; set; }
        public Dictionary<string, string> Settings { get; set; }

    }
}
