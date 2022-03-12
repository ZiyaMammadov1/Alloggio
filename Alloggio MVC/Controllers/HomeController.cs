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
            HomeViewModels HomeVM = new HomeViewModels
            {
                Sliders = _context.Sliders.ToList(),
                Rooms = _context.Rooms.ToList(), 
                Services = _context.Services.ToList(), 
                Subscriptions = _context.Subscriptions.ToList(), 
                Testimonials = _context.Testimonials.ToList(), 
                Settings = _context.Settings.ToDictionary(x=>x.Key, x=>x.Value)

            };
            return View(HomeVM);
        }
        public IActionResult Menu()
        {
            return View();
        }
        public IActionResult Gallery()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
    }
}
