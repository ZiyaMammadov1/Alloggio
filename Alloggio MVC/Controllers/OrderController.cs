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
                CheckIn = DateTime.Parse("3.17.2022"),
                CheckOut = DateTime.Parse("3.17.2022"),
                RoomId = 3,
                Count =1
             }
            };

            Order order = new Order
            {
                AppUserId = "3c5c09e5-7dea-496e-915c-d446aa2bc04c"
            };
              //if(Gelen modeline icerisinde AppUser var ise)
            AppUser member = _userManager.Users.FirstOrDefault(x=>x.Id == order.AppUserId);

            order.FirstName = member.Fullname;
            order.LastName = "";
            order.Phone = member.Phone;
            order.Email = member.Email;
            order.CreateAt = DateTime.UtcNow;
            order.IsDeleted = false;
            order.OrderStatus = Core_Layers.Enums.OrderStatus.Pending;
            order.OrderRooms = rooms;
            order.TotalPrice = 100;


            return View(order);
        }
        public IActionResult  Receipt()
        {
            return View();
        }
    }
}
