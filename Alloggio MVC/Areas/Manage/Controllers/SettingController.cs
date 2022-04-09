using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Alloggio_MVC.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly SettingRepository _settingRepository;

        public SettingController(SettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public IActionResult Index()
        {
            var img1 = _settingRepository.Get(x => x.Key == "AboutImage");
            var img2 = _settingRepository.Get(x => x.Key == "CookingMenuImage");
            var img3 = _settingRepository.Get(x => x.Key == "GalleryImage");
            var img4 = _settingRepository.Get(x => x.Key == "GalleryImage1");
            var img5 = _settingRepository.Get(x => x.Key == "GalleryImage2");
            var img6 = _settingRepository.Get(x => x.Key == "GalleryImage3");
            var img7 = _settingRepository.Get(x => x.Key == "GalleryImage4");
            var img8 = _settingRepository.Get(x => x.Key == "GalleryImage5");
            var img9 = _settingRepository.Get(x => x.Key == "GalleryImage6");
            var img10 = _settingRepository.Get(x => x.Key == "BlogImage");
            var img11 = _settingRepository.Get(x => x.Key == "ContactImage");


            var settings = _settingRepository.GetAll();

           settings.Remove(img1);
           settings.Remove(img2);
           settings.Remove(img3);
           settings.Remove(img4);
           settings.Remove(img5);
           settings.Remove(img6);
           settings.Remove(img7);
           settings.Remove(img8);
           settings.Remove(img9);
           settings.Remove(img10);
           settings.Remove(img11);

            return View(settings);
        }

        public IActionResult Edit(string key)
        {
            var currentSetting = _settingRepository.Get(x => x.Key == key);
            if (currentSetting == null)
            {
                return NotFound();
            }
            return View(currentSetting);
        }

        [HttpPost]
        public IActionResult Edit(Setting setting)
        {
            var currentSetting = _settingRepository.Get(x => x.id == setting.id);

            if (!ModelState.IsValid)
            {
                return View(currentSetting);
            }
            currentSetting.Value = setting.Value;
            _settingRepository.Update(currentSetting);
            _settingRepository.Commit();
            return RedirectToAction("index");
        }
    }
}
