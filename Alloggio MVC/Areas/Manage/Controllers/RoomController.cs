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
    public class RoomController : Controller
    {
        private readonly RoomRepository _roomRepository;
        private readonly BedCountRepository _bedCountRepository;
        private readonly IWebHostEnvironment _env;

        public RoomController(RoomRepository roomRepository, BedCountRepository bedCountRepository, IWebHostEnvironment env)
        {
            _roomRepository = roomRepository;
            _bedCountRepository = bedCountRepository;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_roomRepository.GetAll());
        }

        public IActionResult Create()
        {
            ViewBag.BedCount = _bedCountRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Room Room)
        {
            ViewBag.BedCount = _bedCountRepository.GetAll();

            if (!ModelState.IsValid)
            {
                return View(Room);
            }

            Room NewRoom = new Room();

            NewRoom.Name = Room.Name;
            NewRoom.Size = Room.Size;
            NewRoom.GuestRange = Room.GuestRange;
            NewRoom.Description = Room.Description;
            NewRoom.Price = Room.Price;
            NewRoom.BedCount = Room.BedCount;

            if (Room.MainPhoto.ContentType == "image/jpeg" || Room.MainPhoto.ContentType == "image/png" || Room.MainPhoto.ContentType == "image/jpg")
            {
                if (Room.MainPhoto.Length < 5097152)
                {
                    string NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/Room/RoomMainImage", Room.MainPhoto);
                    NewRoom.Image = NewFileName;
                }
                else
                {
                    ModelState.AddModelError("MainPhoto", "Image size can't be larger than 5mb");
                    return View(Room);
                }
            }
            else
            {
                ModelState.AddModelError("MainPhoto", "Image must be in png, jpg formats");
                return View(Room);
            }


            if (Room.PanoramaPhoto.ContentType == "image/jpeg" || Room.PanoramaPhoto.ContentType == "image/png" || Room.PanoramaPhoto.ContentType == "image/jpg")
            {
                if (Room.PanoramaPhoto.Length < 5097152)
                {
                    string NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/Room/RoomPanoramicPhoto", Room.PanoramaPhoto);
                    NewRoom.PanoramaImage = NewFileName;
                }
                else
                {
                    ModelState.AddModelError("PanoramaPhoto", "Image size can't be larger than 5mb");
                    return View(Room);
                }
            }
            else
            {
                ModelState.AddModelError("PanoramaPhoto", "Image must be in png, jpg formats");
                return View(Room);
            }

            _roomRepository.Add(NewRoom);
            _roomRepository.Commit();


            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Room currentRoom = _roomRepository.Get(id);
            if (currentRoom == null)
            {
                return NotFound();
            }
            FileManager.Delete(_env.WebRootPath, "assets/image/Room/RoomMainImage", currentRoom.Image);
            FileManager.Delete(_env.WebRootPath, "assets/image/Room/RoomPanoramicImage", currentRoom.PanoramaImage);
            _roomRepository.Delete(currentRoom);
            _roomRepository.Commit();
            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Room room = _roomRepository.Get(id);
            if(room == null)
            {
                return NotFound();
            }
            return View(room);
        }
    }
}
