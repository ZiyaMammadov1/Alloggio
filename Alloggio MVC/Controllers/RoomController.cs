using Alloggio_MVC.ViewModels;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Controllers
{
    public class RoomController : Controller
    {
        private readonly DataContext _context;
        public RoomController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Room> rooms = _context.Rooms.ToList();
            return View(rooms);
        }
        public IActionResult RoomDetail(int id)
        {
            
            RoomDetailViewModel RoomDetailMV = new RoomDetailViewModel
            {
                Room = _context.Rooms
                .Include(x => x.RoomAmenities).ThenInclude(x => x.Amenitie)
                .FirstOrDefault(x => x.id == id),
                Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value)

            };
            int CurrentBedCount = RoomDetailMV.Room.BedCount;
            int CurrentRoomId = RoomDetailMV.Room.id;

            RoomDetailMV.RelatedRoom = _context.Rooms
                .Where(x => x.BedCount == CurrentBedCount && x.id != CurrentRoomId)
                .ToList();

            if(RoomDetailMV.Room == null) {
                return RedirectToAction("404 error page");
            }

            return View(RoomDetailMV);
        }

    }
}
