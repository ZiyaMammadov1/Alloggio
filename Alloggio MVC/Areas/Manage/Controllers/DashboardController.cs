using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {
        private readonly SignInManager<AppUser> _signinManager;
        private readonly RoomRepository _roomRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly OrderRepository _orderRepository;

        public DashboardController(SignInManager<AppUser> signinManager, RoomRepository roomRepository, UserManager<AppUser> userManager, OrderRepository orderRepository)
        {
            _signinManager = signinManager;
            _roomRepository = roomRepository;
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            ViewBag.roomsCount = _roomRepository.GetAll().Count;
            ViewBag.Users = _userManager.Users.Where(x=>x.IsAdmin==false).Count();
            ViewBag.Orders = _orderRepository.GetAll(x=>x.CreateAt.Day == DateTime.Now.Day).Where(x=>x.IsDeleted == false).Count();
            var dailyOrder =  _orderRepository.GetAll(x => x.CreateAt.Day == DateTime.Now.Day).Where(x => x.IsDeleted == false).ToList();
            decimal dailyAmount = 0;
            foreach (var order in dailyOrder)
            {
                dailyAmount += order.TotalPrice;
            }
            ViewBag.Amount = dailyAmount;
            List<decimal> Profit = new List<decimal>(11);
            for (int i = 1; i <=12 ; i++)
            {

                decimal profit = 0;
                var orders = _orderRepository.GetAll(x => x.CreateAt.Month == i).Where(x => x.IsDeleted == false).ToList();
                foreach (var order in orders)
                {
                    profit += order.TotalPrice;
                }
                Profit.Insert(i-1, profit);
            }
            ViewBag.Profit = Profit;




            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("login","account");
        }

        public IActionResult NotFound()
        {
            return View();
        }
       
    }
}
