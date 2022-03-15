using Alloggio_MVC.Areas.Manage.ViewModels;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;

        public AccountController(DataContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signManager)
        {
            _context = context;
            _userManager = userManager;
            _signManager = signManager;
        }
        public async Task<IActionResult> Index()
        {
            AppUser Superadmin = new AppUser
            {
                UserName = "Superadmin",
                Fullname = "Super Admin",
                Email = "Superadmin@outlook.com"
            };
            var createresult = await _userManager.CreateAsync(Superadmin, "Admin123");

            return Ok(createresult.Errors);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel AdminLoginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(AdminLoginVM);
            }

            AppUser Admin = await _userManager.FindByNameAsync(AdminLoginVM.Username);

            if (Admin == null)
            {
                ModelState.AddModelError("", "Username or Password incorrect");
                return View(AdminLoginVM);
            }

            if ( !await _userManager.CheckPasswordAsync(Admin, AdminLoginVM.Password))
            {
                ModelState.AddModelError("", "Username or Password incorrect");
                return View(AdminLoginVM);
            }

            var singIn = await _signManager.PasswordSignInAsync(Admin, AdminLoginVM.Password, false, false);

            return RedirectToAction("index","dashboard");
        }
    }
}
