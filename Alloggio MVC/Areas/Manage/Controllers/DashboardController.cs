using Core_Layer.Entities;
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

        public DashboardController(SignInManager<AppUser> signinManager)
        {
            _signinManager = signinManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("login","account");
        }
       
    }
}
