using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class CookingController : Controller
    {
        private readonly CookingRepository _cookingRepository;
        public CookingController(CookingRepository cookingRepository)
        {
            _cookingRepository = cookingRepository;
        }
        public IActionResult Index()
        {
            var cookies = _cookingRepository.GetAll();
            return View(cookies);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CookingMenus cookie)
        {
            if (!ModelState.IsValid)
            {
                return View(cookie);
            }
            CookingMenus newCookie = new CookingMenus()
            {
                FoodName = cookie.FoodName,
                FoodDescription = cookie.FoodDescription
            };
            _cookingRepository.Add(newCookie);
            _cookingRepository.Commit();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var currentCookie = _cookingRepository.Get(x => x.id == id);
            if (currentCookie == null)
            {
                return RedirectToAction("notfound", "dashboard", "manage");
            }
            return View(currentCookie);
        }

        [HttpPost]
        public IActionResult Edit(CookingMenus menu)
        {
            if (!ModelState.IsValid)
            {
                return View(menu);
            }
            var currentCookie = _cookingRepository.Get(x => x.id == menu.id);
            if (currentCookie == null)
            {
                return RedirectToAction("notfound", "dashboard", "manage");
            }
            currentCookie.FoodName = menu.FoodName;
            currentCookie.FoodDescription = menu.FoodDescription;
            _cookingRepository.Update(currentCookie);
            _cookingRepository.Commit();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var currentCookie = _cookingRepository.Get(x=>x.id == id);
            if(currentCookie == null)
            {
                return RedirectToAction("notfound", "dashboard", "manage");
            }
            _cookingRepository.Delete(currentCookie);
            _cookingRepository.Commit();

            return RedirectToAction("index");
        }
    }
}
