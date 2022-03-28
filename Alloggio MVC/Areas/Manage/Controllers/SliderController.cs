using Core_Layer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly SliderService _sliderService;

        public SliderController(SliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public IActionResult Index()
        {
            return View(_sliderService.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return RedirectToAction("index");
        }
    }
}
