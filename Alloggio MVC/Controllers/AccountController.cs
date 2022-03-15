using Alloggio_MVC.Helpers;
using Alloggio_MVC.Helpers.EmailSender;
using Alloggio_MVC.Helpers.FileManager;
using Alloggio_MVC.ViewModels;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _env;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DataContext context, IWebHostEnvironment env, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _env = env;
            _emailSender = emailSender;
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

            if (RegisterVm.UploadImage != null)
            {
                if (RegisterVm.UploadImage.ContentType == "image/jpeg" || RegisterVm.UploadImage.ContentType == "image/png" || RegisterVm.UploadImage.ContentType == "image/jpg")
                {
                    if (RegisterVm.UploadImage.Length < 2097152)
                    {
                        string NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/account", RegisterVm.UploadImage);
                        member.Image = NewFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("UploadImage", "Image size can't be larger than 2mb");
                    }
                }
                else
                {
                    ModelState.AddModelError("UploadImage", "Image must be in png, jpg formats");
                    return View();
                }


            }


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

            TempData["Success"] = "Create your profile";

            return RedirectToAction("login", "account");
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

            AppUser UserModel = await _userManager.FindByNameAsync(UpdateVM.CurrentMemberName);

            UserModel.Fullname = UpdateVM.Fullname;
            UserModel.Email = UpdateVM.Email;
            UserModel.Phone = UpdateVM.Phone;
            if (UpdateVM.UploadImage == null)
            {
                UpdateVM.Image = UserModel.Image;
            }

            if (!ModelState.IsValid)
            {
                return View(UpdateVM);
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


            string NewFileName = "";

            if (UpdateVM.UploadImage != null)
            {

                if (UpdateVM.UploadImage.ContentType == "image/jpeg" || UpdateVM.UploadImage.ContentType == "image/png" || UpdateVM.UploadImage.ContentType == "image/jpg")
                {

                    UpdateVM.Image = UserModel.Image;
                    if (UpdateVM.UploadImage.Length < 2097152)
                    {
                        NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/account", UpdateVM.UploadImage);
                        FileManager.Delete(_env.WebRootPath, "assets/image/account", UserModel.Image);
                        UserModel.Image = NewFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("UploadImage", "Image size can't be larger than 2mb");
                        return View(UpdateVM);
                    }
                }
                else
                {

                    UpdateVM.Image = UserModel.Image;
                    ModelState.AddModelError("UploadImage", "Image must be in png, jpg formats");
                    return View(UpdateVM);
                }


            }
            else
            {
                UpdateVM.Image = UserModel.Image;
            }

            _context.SaveChanges();

            TempData["Success"] = "Profile update successfully";

            return RedirectToAction("profile", "account");
        }



        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
        {
            if (string.IsNullOrEmpty(forgotPasswordVM.Email))
            {
                ModelState.AddModelError("Email","Enter email address");
                return View();
            }
            AppUser member = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);

            if(member == null)
            {
                ModelState.AddModelError("Email", "No user were found matching this email");
                return View();
            }

            var ResetCode = await _userManager.GeneratePasswordResetTokenAsync(member);

            var ResetUrl = Url.Action("ResetPassword","Account", new { 
                userid = member.Id,
                token = ResetCode
            });

            await _emailSender.SendEmailAsync(forgotPasswordVM.Email, "Alloggio Hotel", $"<a href='https://localhost:44323{ResetUrl}'>Click</a> for change Password");
            return RedirectToAction("index","home");
        }



        public IActionResult ResetPassword(string userid, string token)
        {
            if(userid == null || token == null)
            {
                return RedirectToAction("index","home");
            }
            var model = new ChangePasswordViewModel { Token = token };
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ChangePasswordViewModel ChangePasswordVm)
        {
            if (!ModelState.IsValid)
            {
                return View(ChangePasswordVm);
            }
            var user = await _userManager.FindByEmailAsync(ChangePasswordVm.Email);
            if(user == null)
            {
                ModelState.AddModelError("","User not found");
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(user, ChangePasswordVm.Token, ChangePasswordVm.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                    return View();
                }
            }
            TempData["Success"] = "Password changed";

            return RedirectToAction("login","account");
        }



        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

    }
}
