using Alloggio_MVC.Helpers.Payment;
using Alloggio_MVC.ViewModels;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
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
            List<Core_Layer.Entities.BasketItem> data = new List<Core_Layer.Entities.BasketItem>();
            if (User.Identity.IsAuthenticated)
            {
                AppUser member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                data = _context.BasketItems.Include(x => x.Room).Where(x => x.AppUserId == member.Id).ToList();
                return View(data);
            }

            return View(data);
        }

       
        public IActionResult DeleteOrder(int id)
        {
            Core_Layer.Entities.BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.Id == id);
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
            if(checkIn < DateTime.Now || checkOut < DateTime.Now || checkIn > checkOut)
            {
                return RedirectToAction("Index","Home");
            }
            if (member != null)
            {
                Core_Layer.Entities.BasketItem item = _context.BasketItems.FirstOrDefault(x => x.AppUserId == member.Id && x.RoomId == id);

                if (item == null)
                {
                    item = new Core_Layer.Entities.BasketItem
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
            AppUser member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (member == null)
            {
                return NotFound();
            }
            Order order = new Order
            {
                AppUser = member
            };
            PaymentViewModel paymentViewModel = new PaymentViewModel
            {
                Order = order
            };
            return View(paymentViewModel);
        }

        private BasketViewModel _getBasket(AppUser member)
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                BasketItems = new List<RoomBasketViewModel>()
            };

            List<BasketItemViewModel> items = new List<BasketItemViewModel>();


            items = _context.BasketItems.Where(x => x.AppUserId == member.Id).Select(c => new BasketItemViewModel { Roomid = c.RoomId, Count = c.Count, CheckIn = c.CheckIn, CheckOut = c.CheckOut, Adult = c.Adults, Children = c.Childrens, Infant = c.Infants }).ToList();



            foreach (var item in items)
            {
                Room room = _context.Rooms.FirstOrDefault(x => x.id == item.Roomid);
                RoomBasketViewModel bookItem = new RoomBasketViewModel
                {
                    Room = room,
                    Count = item.Count,
                    CheckIn = item.CheckIn,
                    CheckOut = item.CheckOut,
                    Adult = item.Adult,
                    Children = item.Children,
                    Infant = item.Infant
                };

                double totalPrice = room.Price;
                basketVM.TotalPrice += totalPrice * item.Count;

                basketVM.BasketItems.Add(bookItem);
            }

            return basketVM;
        }

        [HttpPost]
        public IActionResult Checkout(PaymentViewModel PaymentVm)
        {
            AppUser member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (member == null)
            {
                return NotFound();
            }

            List<Core_Layer.Entities.BasketItem> BasketItems = _context.BasketItems.Include(x => x.Room).Where(x => x.AppUserId == member.Id).ToList();


            CheckoutViewModel checkoutVM = new CheckoutViewModel
            {
                Basket = _getBasket(member),
                Order = PaymentVm.Order
            };

            if (!ModelState.IsValid || PaymentVm.CardNumber ==null || PaymentVm.ExpiredYear == null|| PaymentVm.ExpiredMonth == null|| PaymentVm.CardCVC == null)
            { 
                ModelState.AddModelError("", "Fill all inputs!");
                return View("Checkout", PaymentVm);
            }

            if(PaymentVm.CardNumber.Length < 16)
            {
                ModelState.AddModelError("", "Card number must be 16 character!");
                return View("Checkout", PaymentVm);
            }

            if (checkoutVM.Basket.BasketItems.Count == 0)
            {
                ModelState.AddModelError("", "You must choose any product!");
                return View("Checkout", checkoutVM.Order);
            }

            Payments newPayment = new Payments();
            var result = newPayment.payment();

            if (result.Status == "success")
            {
                TempData["Success"] = "Payment successfly";

            }
            else
            {
                TempData["Warning"] = "Payment not succesfly";

            } 
           



            PaymentVm.Order.AppUserId = member?.Id;
            PaymentVm.Order.CreateAt = DateTime.UtcNow.AddHours(4);
            PaymentVm.Order.ModifiedAt = DateTime.UtcNow.AddHours(4);
            PaymentVm.Order.OrderStatus = Core_Layers.Enums.OrderStatus.Pending;



            PaymentVm.Order.OrderRooms = new List<OrderRooms>();

            foreach (var item in checkoutVM.Basket.BasketItems)
            {
                OrderRooms orderItem = new OrderRooms
                {
                    RoomId = item.Room.id,
                    Price = item.Room.Price,
                    Count = item.Count,
                    CheckIn = item.CheckIn,
                    CheckOut = item.CheckOut,
                    Adult = item.Adult,
                    Children = item.Children,
                    Infant = item.Infant
                };

                PaymentVm.Order.OrderRooms.Add(orderItem);
                PaymentVm.Order.TotalPrice += orderItem.Price * orderItem.Count;
            }

            _context.Orders.Add(PaymentVm.Order);

            foreach (var item in BasketItems)
            {
                _context.BasketItems.Remove(item);

            }

            _context.SaveChanges();

            return RedirectToAction("profile", "account");
        }
      
    }
}
