using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Controllers
{
    public class OrderController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;

        public OrderController(UserManager<AppUser> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Basket()
        {
            List<BasketItem> data = new List<BasketItem>();
            if (User.Identity.IsAuthenticated)
            {
                AppUser member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                data = _context.BasketItems.Include(x=>x.Room).Where(x => x.AppUserId == member.Id).ToList();
                return View(data);
            }
        
            return View(data);
        }

        public IActionResult DeleteOrder(int id)
        {
            BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.Id == id);
            if (basketItem != null)
            {
                _context.BasketItems.Remove(basketItem);
                _context.SaveChanges();
            }

            return RedirectToAction("basket", "order");
        }

        public IActionResult AddBasket(int adults, int childrens, int infants, DateTime checkIn, DateTime checkOut, int id)
        {
            ViewBag.Checkin = checkIn;
            ViewBag.Checkout = checkOut;
            ViewBag.Adults = adults;
            ViewBag.Childrens = childrens;
            ViewBag.Infants = infants;

            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }
            if (member != null)
            {
                BasketItem item = _context.BasketItems.FirstOrDefault(x => x.AppUserId == member.Id && x.RoomId == id);

                if (item == null)
                {
                    item = new BasketItem
                    {
                        AppUserId = member.Id,
                        RoomId = id,
                        CreatedAt = DateTime.UtcNow.AddHours(4),
                        Count = 1,
                        CheckIn = checkIn,
                        CheckOut = checkOut,
                        Adults = adults,
                        Childrens = childrens,
                        Infants = infants
                    };
                    _context.BasketItems.Add(item);
                }
                else
                    item.Count++;

                _context.SaveChanges();

                var items = _context.BasketItems.Where(x => x.AppUserId == member.Id).ToList();
            }
            else
            {
                return RedirectToAction("login", "account");
            }
            return RedirectToAction("index", "home");
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
