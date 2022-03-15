using Alloggio_MVC.Areas.Manage.ViewModels;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccountController(DataContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
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

            AppUser Admin = await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName == AdminLoginVM.Username && x.IsAdmin);

            if (Admin == null)
            {
                ModelState.AddModelError("", "Username or Password incorrect");
                return View(AdminLoginVM);
            }

            if (!await _userManager.CheckPasswordAsync(Admin, AdminLoginVM.Password))
            {
                ModelState.AddModelError("", "Username or Password incorrect");
                return View(AdminLoginVM);
            }

            var singIn = await _signManager.PasswordSignInAsync(Admin, AdminLoginVM.Password, false, false);

            return RedirectToAction("index", "dashboard");
        }
    }
}
