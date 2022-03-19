using Core_Layer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public OrderController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Basket()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            List<OrderRooms> rooms = new List<OrderRooms>()
            {
             new OrderRooms()
             {
                CheckIn = DateTime.Now,
                CheckOut = DateTime.Now,
                RoomId = 3,
                Count =1
             }
            };

            //if(Gelen modeline icerisinde AppUser var ise)
            AppUser member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            Order order = new Order()
            {
                FullName = member.Fullname,
                Phone = member.Phone,
                Email = member.Email,
                CreateAt = DateTime.UtcNow,
                IsDeleted = false,
                OrderStatus = Core_Layers.Enums.OrderStatus.Pending,
                OrderRooms = rooms,
                TotalPrice = 100
            };

            return View(order);
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            return Ok(order);
        }
        public IActionResult Receipt()
        {
            return View();
        }
    }
}
