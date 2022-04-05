using Alloggio_MVC.Helpers.FileManager;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class GalleryController : Controller
    {
        private readonly SettingRepository _settingRepository;
        private readonly IWebHostEnvironment _env;

        public GalleryController(SettingRepository settingRepository, IWebHostEnvironment env)
        {
            _settingRepository = settingRepository;
            _env = env;
        }
        public IActionResult Index()
        {
            List<String> AllImage = new List<string>();
            var image1 = _settingRepository.GetValue("GalleryImage1");
            var image2 = _settingRepository.GetValue("GalleryImage2");
            var image3 = _settingRepository.GetValue("GalleryImage3");
            var image4 = _settingRepository.GetValue("GalleryImage4");
            var image5 = _settingRepository.GetValue("GalleryImage5");
            var image6 = _settingRepository.GetValue("GalleryImage6");

            AllImage.Add(image1);
            AllImage.Add(image2);
            AllImage.Add(image3);
            AllImage.Add(image4);
            AllImage.Add(image5);
            AllImage.Add(image6);

            return View(AllImage);
        }

        public IActionResult Edit(string key)
        {
            var image = _settingRepository.Get(x => x.Value == key);

            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost]
        public IActionResult Edit(Setting setting)
        {
            var currentSetting = _settingRepository.Get(x => x.id == setting.id);

            if (currentSetting == null)
            {
                return NotFound();
            }
            if (setting.Photo == null)
            {
                return RedirectToAction("index");
            }
            string NewFileName = "";
            if (setting.Photo.ContentType == "image/jpeg" || setting.Photo.ContentType == "image/png" || setting.Photo.ContentType == "image/jpg")
            {

                if (setting.Photo.Length < 2097152)
                {
                    NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/Gallery", setting.Photo);
                    FileManager.Delete(_env.WebRootPath, "assets/image/Gallery", currentSetting.Value);
                    currentSetting.Value = NewFileName;
                    _settingRepository.Update(currentSetting);
                    _settingRepository.Commit();
                }
                else
                {
                    ModelState.AddModelError("", "Image size can't be larger than 2mb");
                    return View(setting);
                }

            }
            else
            {
                ModelState.AddModelError("", "Photo is not correct type Ex: (.png, .jpg)");
                return View(setting);
            }
            return RedirectToAction("index");
        }
    }
}
