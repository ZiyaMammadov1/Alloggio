using Alloggio_MVC.ViewModels;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        public HomeController(DataContext context)
        {
            _context = context;
            
        }


        public IActionResult Index()
        {
            ViewBag.SelectRoom = _context.BedCount.ToList();
            Checking checkForm = new Checking
            {
                CheckIn = DateTime.UtcNow,
                CheckOut = DateTime.UtcNow.AddDays(1),
                
            };

            HomeViewModels HomeVM = new HomeViewModels
            {
                Sliders = _context.Sliders.ToList(),
                Rooms = _context.Rooms.Take(4).ToList(),
                Services = _context.Services.ToList(),
                Testimonials = _context.Testimonials.ToList(),
                Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value),
                MainChecking = checkForm

            };
            return View(HomeVM);
        }

        [HttpPost]
        public IActionResult Index(HomeViewModels HomeVM)
        {

            if (HomeVM.SubscriptionEmail != null)
            {
                Subscription email = new Subscription
                {
                    Email = HomeVM.SubscriptionEmail
                };

                _context.Subscriptions.Add(email);
                _context.SaveChanges();

                return RedirectToAction("index", "home");

            }
            HomeViewModels newHomeVM = new HomeViewModels
            {
                Sliders = _context.Sliders.ToList(),
                Rooms = _context.Rooms.Take(4).ToList(),
                Services = _context.Services.ToList(),
                Testimonials = _context.Testimonials.ToList(),
                Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value)

            };
            return View(newHomeVM);
        }

        public IActionResult Menu()
        {
            CookingMenuViewModel cookingVm = new CookingMenuViewModel
            {
                Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value),
                Menus = _context.CookingMenus.ToList()
            };
            return View(cookingVm);
        }

        public IActionResult Gallery()
        {
             var ModelForGallery = _context.Settings.ToDictionary(x => x.Key, x => x.Value);

            return View(ModelForGallery);
        }

        public IActionResult NotFound()
        {
            return View();
        }
    }
}
