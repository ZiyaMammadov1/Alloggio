using Alloggio_MVC.ViewModels;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Controllers
{
    public class BlogController : Controller
    {
        private readonly DataContext _context;
        public BlogController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            BlogViewModel BlogVm = new BlogViewModel
            {
                Blogs = _context.Blogs.ToList(),
                Settings = _context.Settings.ToDictionary(x=>x.Key, x=>x.Value)
            };
            return View(BlogVm);
        }
        public IActionResult Detail(int id)
        {
            var CurrentBlog = _context.Blogs.FirstOrDefault(x=>x.id == id);
            if(CurrentBlog == null)
            {
                return RedirectToAction("404 Error Page");
            }
            return View(CurrentBlog);
        }
    }
}
