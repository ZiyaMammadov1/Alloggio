using Alloggio_MVC.ViewModels;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Helpers.LayoutService
{
    public class LayoutService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;



        public LayoutService(DataContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }


        public int GetBasket(string UserName)
        {
            if (!string.IsNullOrEmpty(UserName)) { 
                AppUser member = _userManager.Users.FirstOrDefault(x => x.UserName == UserName);

                var BasketMemory = _context.BasketItems.Where(x=>x.AppUserId == member.Id).ToList();

                return BasketMemory.Count;
            }
            else
            {
                return 0;
            }

        }


    }
}
