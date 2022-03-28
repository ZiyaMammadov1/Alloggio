using Core_Layer.Abstract;
using Core_Layer.Entities;
using Data_Layer.Concrete;
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
        private readonly SliderRepository _sliderRepository;

        public SliderController(SliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }

        public IActionResult Index()
        {
            return View(_sliderRepository.GetAll());
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
