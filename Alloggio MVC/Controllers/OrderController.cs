using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Basket()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult  Receipt()
        {
            return View();
        }
    }
}
