using Alloggio_MVC.Helpers.FileManager;
using Core_Layer.Abstract;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _env;

        public SliderController(SliderRepository sliderRepository, IWebHostEnvironment env)
        {
            _sliderRepository = sliderRepository;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_sliderRepository.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider newSlider)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Add background image");
                return View(newSlider);
            }

            Slider slider = new Slider
            {
                Header = newSlider.Header
            };

            if (newSlider.ImageFile.ContentType == "image/jpeg" || newSlider.ImageFile.ContentType == "image/png" || newSlider.ImageFile.ContentType == "image/jpg")
            {
                if (newSlider.ImageFile.Length < 5097152)
                {
                    string NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/slider", newSlider.ImageFile);
                    slider.Image = NewFileName;
                }
                else
                {
                    ModelState.AddModelError("Image", "Image size can't be larger than 5mb");
                    return View(newSlider);
                }
            }
            else
            {
                ModelState.AddModelError("Image", "Image must be in png, jpg formats");
                return View(newSlider);
            }

            _sliderRepository.Add(slider);
            _sliderRepository.Commit();

            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Slider currentSlider = _sliderRepository.Get(id);

            return View(currentSlider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            Slider currentSlider = _sliderRepository.Get(slider.id);

            if (currentSlider == null)
            {
                ModelState.AddModelError("", "Slider not found");
                return View(slider);
            }

            currentSlider.Header = slider.Header;

            if (slider.ImageFile != null)
            {
                if (slider.ImageFile.Length < 5097152)
                {
                    string NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/slider", slider.ImageFile);
                    FileManager.Delete(_env.WebRootPath, "assets/image/slider", currentSlider.Image);
                    currentSlider.Image = NewFileName;
                }
                else
                {
                    ModelState.AddModelError("Image", "Image size can't be larger than 5mb");
                    return View(slider);
                }
            }


            _sliderRepository.Update(currentSlider);
            _sliderRepository.Commit();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Slider currentSlider = _sliderRepository.Get(id);

            if (currentSlider == null)
            {
                return NotFound();
            }
            _sliderRepository.Delete(currentSlider);
            _sliderRepository.Commit();

            return RedirectToAction("index");
        }
    }
}
