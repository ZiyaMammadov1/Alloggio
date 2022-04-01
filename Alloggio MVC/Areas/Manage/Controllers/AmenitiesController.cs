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
    public class AmenitiesController : Controller
    {
        private readonly AmenitiesRepository _amenitiesRepository;

        public AmenitiesController(AmenitiesRepository amenitiesRepository)
        {
            _amenitiesRepository = amenitiesRepository;
        }

        public IActionResult Index()
        {

            return View(_amenitiesRepository.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Amenitie amenity)
        {
            if (!ModelState.IsValid)
            {
                return View(amenity);
            }
            var currentAmenity = _amenitiesRepository.GetAll(x => x.Name == amenity.Name);
            if(currentAmenity == null)
            {
                ModelState.AddModelError("","Amenity is exist");
                return View(amenity);
            }
            Amenitie newAmenity = new Amenitie
            {
                Name = amenity.Name
            };

            string modifiedName = amenity.Image;
            string newImageName = modifiedName.Replace("class=\"", "class =\"detailWrap_Icon ");

            newAmenity.Image = newImageName;

            _amenitiesRepository.Add(newAmenity);
            _amenitiesRepository.Commit();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Amenitie currentAmenity = _amenitiesRepository.Get(x=>x.id == id);
            if(currentAmenity == null)
            {
                return NotFound();
            }
            return View(currentAmenity);
        }

        [HttpPost]
        public IActionResult Edit(Amenitie amenitie)
        {
            Amenitie currentAmenity = _amenitiesRepository.Get(x=>x.id == amenitie.id);

            if(currentAmenity == null)
            {
                ModelState.AddModelError("","Amenity not found");
                return View(amenitie);
            }

            currentAmenity.Image = amenitie.Image;
            currentAmenity.Name = amenitie.Name;
            _amenitiesRepository.Update(currentAmenity);
            _amenitiesRepository.Commit();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Amenitie currentAmenity = _amenitiesRepository.Get(x => x.id == id);
            if (currentAmenity == null)
            {
                return NotFound();
            }

            _amenitiesRepository.Delete(currentAmenity);
            _amenitiesRepository.Commit();
            return RedirectToAction("index");
        }
    }
}
