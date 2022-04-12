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

            AppUser Admin = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == AdminLoginVM.Username && x.IsAdmin);

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

            if (!singIn.Succeeded)
            {
                ModelState.AddModelError("", "Some problem occured");
                return View(AdminLoginVM);
            }

            return RedirectToAction("index", "dashboard");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetUser()
        {
            List<AppUser> users = _userManager.Users.Where(x => x.IsDelete == false).ToList();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentUser = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (currentUser == null)
            {
                return RedirectToAction("notfound", "dashboard", "manage");
            }
            currentUser.IsDelete = true;
           await _userManager.UpdateAsync(currentUser);
          
            return  RedirectToAction("getuser");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetComment(string id)
        {
            var Comments = _context.UserComments.Include(x=>x.Room).Include(x=>x.AppUser).Where(x => x.AppUserId == id).ToList();
            return View(Comments);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteComment(int id)
        {
            
            var comment = _context.UserComments.Include(x=>x.AppUser).FirstOrDefault(x=>x.Id == id);
            string key = comment.AppUser.Id;
            if (comment == null)
            {
                return RedirectToAction("notfound", "dashboard", "manage");
            }
            _context.UserComments.Remove(comment);
            _context.SaveChanges();

            return RedirectToAction("GetComment", new { id = key});

        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetOrder(string id)
        {
            var Orders = _context.Orders.Where(x => x.AppUserId == id && x.IsDeleted == false).ToList();
            return View(Orders);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditUser(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x=>x.Id == id);
            if(user == null)
            {
                return RedirectToAction("notfound", "dashboard", "manage");
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditUser(AppUser user)
        {
            var currentUser = _userManager.Users.FirstOrDefault(x=>x.Id== user.Id);
            if(currentUser == null)
            {
                return RedirectToAction("notfound", "dashboard", "manage");
            }
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            if(user.Email == null)
            {
                ModelState.AddModelError("Email", "Email is required");
                return View(user);
            }

            currentUser.Fullname = user.Fullname;
            currentUser.Email = user.Email;
            currentUser.Phone = user.Phone;

            if(user.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(currentUser);
                var result = await _userManager.ResetPasswordAsync(currentUser, token, user.Password);
                    
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                        return View(user);
                    }
                }
            }

            _context.AppUsers.Update(currentUser);
            _context.SaveChanges();

            return RedirectToAction("getuser");
        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(AppUser user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            AppUser newAdmin = new AppUser
            {
                Fullname = user.UserName,
                Email = user.Email,
                IsAdmin = true,
                Phone = user.Phone,
                EmailConfirmed = true,
                UserName = user.UserName
            };
            var createProcess = await _userManager.CreateAsync(newAdmin);
            if (!createProcess.Succeeded)
            {
                foreach (var error in createProcess.Errors)
                {
                    ModelState.AddModelError("UserName",error.Description);
                    return View(user);
                }
            }

            var Admin = _userManager.Users.FirstOrDefault(x=>x.UserName == newAdmin.UserName);

            if(Admin == null)
            {
                return RedirectToAction("notfound", "dashboard", "manage");
            } 

            var defaultPasswordAdded = await _userManager.AddPasswordAsync(Admin, user.Password);
            
            if (!defaultPasswordAdded.Succeeded)
            {
                foreach (var error in createProcess.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(user);
                }
            }
            var AgainAdmin = _userManager.Users.FirstOrDefault(x => x.UserName == newAdmin.UserName);

            var AddRoleOnAdmin = await _userManager.AddToRoleAsync(AgainAdmin, "Admin");

            

            if (!AddRoleOnAdmin.Succeeded)
            {
                foreach (var error in createProcess.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(user);
                }
            }



            return RedirectToAction("login","account");
        }
    }
}
