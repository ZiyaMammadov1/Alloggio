using Alloggio_MVC.ViewModels;
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
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterViewModel RegisterVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser member = await _userManager.FindByNameAsync(RegisterVm.Username);

            if (member != null)
            {
                ModelState.AddModelError("Username", "Username already exist");
                return View();
            }

            member = new AppUser
            {
                Fullname = RegisterVm.Fullname,
                Email = RegisterVm.Email,
                UserName = RegisterVm.Username,
                Phone = RegisterVm.Phone

            };
            var result = await _userManager.CreateAsync(member, RegisterVm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await _signInManager.SignInAsync(member, true);

            return RedirectToAction("index", "home");
        }





        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel LoginVM)
        {
            if (string.IsNullOrEmpty(LoginVM.Email) || string.IsNullOrEmpty(LoginVM.Password))
            {
                ModelState.AddModelError("", "Fill all area");
                return View();
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Email or Password is incorrect ");
                return View();
            }

            AppUser member = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == LoginVM.Email);

            if (member == null)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(member, LoginVM.Password, true, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View();
            }

            return RedirectToAction("index", "home");
        }




        public async Task<IActionResult> Profile()
        {
            AppUser UserModel = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            if (UserModel == null)
            {
                ModelState.AddModelError("", "Member not found");
                return View();
            }
            MemberUpdateViewModel MemberModel = new MemberUpdateViewModel
            {
                Email = UserModel.Email,
                Fullname = UserModel.Fullname,
                Phone = UserModel.Phone,
                Image = UserModel.Image

            };
            return View(MemberModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(MemberUpdateViewModel UpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(UpdateVM);
            }

            AppUser UserModel = await _userManager.FindByNameAsync(UpdateVM.CurrentMemberName);

            UserModel.Fullname = UpdateVM.Fullname;
            UserModel.Email = UpdateVM.Email;
            UserModel.Phone = UpdateVM.Phone;

            if (string.IsNullOrEmpty(UpdateVM.Image))
            {
                UpdateVM.Image = UserModel.Image;
            }

            if (!string.IsNullOrEmpty(UpdateVM.OldPassword))
            {
                if (string.IsNullOrEmpty(UpdateVM.NewPassword) && string.IsNullOrEmpty(UpdateVM.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Fill all password inputs");
                    return View(UpdateVM);
                }
                if (await _userManager.CheckPasswordAsync(UserModel, UpdateVM.OldPassword))
                {
                    if (UpdateVM.NewPassword.Trim() == UpdateVM.ConfirmPassword.Trim())
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(UserModel);
                        var result = await _userManager.ResetPasswordAsync(UserModel, token, UpdateVM.NewPassword);

                        if (!result.Succeeded)
                        {
                            foreach (var err in result.Errors)
                            {
                                ModelState.AddModelError("", err.Description);
                            }
                            return View(UpdateVM);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Old Password is incorrect");
                    return View(UpdateVM);
                }

            }


            var processUpdate = await _userManager.UpdateAsync(UserModel);

            if (!processUpdate.Succeeded)
            {
                foreach (var err in processUpdate.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View(UpdateVM);
            }

            _context.SaveChanges();


            return RedirectToAction("index", "home");
        }




        public IActionResult ForgotPassword()
        {
            return View();
        }



        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

    }
}
