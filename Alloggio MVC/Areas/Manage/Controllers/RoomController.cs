using Alloggio_MVC.Helpers.FileManager;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly AmenitiesRepository _amenitiesRepository;
        private readonly RoomAmenitiesRepository _roomAmenities;
        private readonly IWebHostEnvironment _env;

        public RoomController(RoomRepository roomRepository, BedCountRepository bedCountRepository, IWebHostEnvironment env, AmenitiesRepository amenitiesRepository, RoomAmenitiesRepository roomAmenities)
        {
            _roomRepository = roomRepository;
            _bedCountRepository = bedCountRepository;
            _env = env;
            _amenitiesRepository = amenitiesRepository;
            _roomAmenities = roomAmenities;
        }

        public IActionResult Index()
        {
            return View(_roomRepository.GetAll());
        }

        public IActionResult Create()
        {
            ViewBag.BedCount = _bedCountRepository.GetAll();
            ViewBag.Amenities = _amenitiesRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Room Room)
        {
            ViewBag.BedCount = _bedCountRepository.GetAll();
            ViewBag.Amenities = _amenitiesRepository.GetAll();

            if (!ModelState.IsValid)
            {
                return View(Room);
            }

            if(Room.MainPhoto == null || Room.PanoramaPhoto == null)
            {
                ModelState.AddModelError("","Main Photo and Panoromic Photo is required");
                return View(Room);
            }

            Room NewRoom = new Room();

            NewRoom.Name = Room.Name;
            NewRoom.Size = Room.Size;
            NewRoom.GuestRange = Room.GuestRange;
            NewRoom.Description = Room.Description;
            NewRoom.Price = Room.Price;
            NewRoom.BedCount = Room.BedCount;

            if(Room.AmenitiesIds != null)
            {
                foreach (var ids in Room.AmenitiesIds)
                {
                    if (_amenitiesRepository.GetAll().Any(x => x.id == ids))
                    {
                        RoomAmenities NewAmenities = new RoomAmenities
                        {
                            Amenitieid = ids,
                            Roomid = Room.id
                        };

                        NewRoom.RoomAmenities.Add(NewAmenities);
                    }
                    else
                    {
                        return RedirectToAction("notfound", "dashboard", "manage");
                    }
                }
            }
            
            

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
                return RedirectToAction("notfound", "dashboard", "manage");
            }
            FileManager.Delete(_env.WebRootPath, "assets/image/Room/RoomMainImage", currentRoom.Image);
            FileManager.Delete(_env.WebRootPath, "assets/image/Room/RoomPanoramicImage", currentRoom.PanoramaImage);
            _roomRepository.Delete(currentRoom);
            _roomRepository.Commit();
            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.BedCount = _bedCountRepository.GetAll();
            ViewBag.Amenities = _amenitiesRepository.GetAll();
            ViewBag.RoomAmenities = _roomAmenities.GetAll(x => x.Roomid == id).Include(x => x.Amenitie).ToList();


            Room room = _roomRepository.Get(id);

            if (room == null)
            {
                return RedirectToAction("notfound", "dashboard", "manage");
            }

            room.AmenitiesIds = room.RoomAmenities.Select(c => c.Amenitieid).ToList();

            return View(room);
        }

        [HttpPost]
        public IActionResult Edit(Room room)
        {
            ViewBag.BedCount = _bedCountRepository.GetAll();
            ViewBag.Amenities = _amenitiesRepository.GetAll();
            ViewBag.RoomAmenities = _roomAmenities.GetAll(x => x.Roomid == room.id).Include(x => x.Amenitie).ToList();

            if (!ModelState.IsValid)
            {
                return View(room);
            }

            Room currentRoom = _roomRepository.Get(room.id);

            if(currentRoom == null)
            {
                return RedirectToAction("notfound", "dashboard", "manage");
            }

            currentRoom.Name = room.Name;
            currentRoom.Size = room.Size;
            currentRoom.GuestRange = room.GuestRange;
            currentRoom.Description = room.Description;
            currentRoom.Price = room.Price;
            currentRoom.BedCount = room.BedCount;

            var currentAmenities = currentRoom.RoomAmenities.ToList();

            foreach (var amenity in currentAmenities)
            {
                _roomAmenities.Delete(amenity);
                _roomAmenities.Commit();
            }

            var NewAmenitiesIds = room.AmenitiesIds.ToList();

            foreach (var AmenityId in NewAmenitiesIds)
            {
                RoomAmenities newAmenity = new RoomAmenities
                {
                    Roomid = currentRoom.id,
                    Amenitieid = AmenityId
                };

                _roomAmenities.Add(newAmenity);
                _roomRepository.Commit();
            }

            if (room.MainPhoto != null)
            {
                if (room.MainPhoto.ContentType == "image/jpeg" || room.MainPhoto.ContentType == "image/png" || room.MainPhoto.ContentType == "image/jpg")
                {
                    if (room.MainPhoto.Length < 5097152)
                    {
                        string NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/Room/RoomMainImage", room.MainPhoto);
                        FileManager.Delete(_env.WebRootPath, "assets/image/Room/RoomMainImage", currentRoom.Image);
                        currentRoom.Image = NewFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("MainPhoto", "Image size can't be larger than 5mb");
                        return View(room);
                    }
                }
                else
                {
                    ModelState.AddModelError("MainPhoto", "Image must be in png, jpg formats");
                    return View(room);
                }
            }
           


            if (room.PanoramaPhoto != null)
            {
                if (room.PanoramaPhoto.ContentType == "image/jpeg" || room.PanoramaPhoto.ContentType == "image/png" || room.PanoramaPhoto.ContentType == "image/jpg")
                {
                    if (room.PanoramaPhoto.Length < 5097152)
                    {
                        string NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/Room/RoomPanoramicPhoto", room.PanoramaPhoto);
                        FileManager.Delete(_env.WebRootPath, "assets/image/Room/RoomPanoramicPhoto", currentRoom.PanoramaImage);
                        currentRoom.PanoramaImage = NewFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("PanoramaPhoto", "Image size can't be larger than 5mb");
                        return View(room);
                    }
                }
                else
                {
                    ModelState.AddModelError("PanoramaPhoto", "Image must be in png, jpg formats");
                    return View(room);
                }
            }
            

            _roomRepository.Update(currentRoom);
            _roomRepository.Commit();

            return RedirectToAction("index");
        }
    }
}
