using Alloggio_MVC.ViewModels;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        public RoomController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(int adults, int childrens, int infants, DateTime checkin, DateTime checkout, int room)
        {
            ViewBag.Checkin = checkin;
            ViewBag.Checkout = checkout;
            ViewBag.Adults = adults;
            ViewBag.Childrens = childrens;
            ViewBag.Infants = infants;

            List<Room> AllRoom = _context.Rooms.ToList();
            List<Room> roomlistforBedCount = _context.Rooms
                .Where(x => x.BedCount == room).ToList();
            List<OrderRooms> AvailableRoomsId = new List<OrderRooms>();
            List<Room> AvailableRooms = new List<Room>();
            List<OrderRooms> SelectOrderRoom = new List<OrderRooms>();
            List<OrderRooms> UnregisteredOrderRoom = new List<OrderRooms>();
            if (roomlistforBedCount != null)
            {
                SelectOrderRoom = _context.OrderRooms
                     .Where(x => x.Room.BedCount == room)
                     .OrderByDescending(x => x.id)
                     .Take(50)
                     .ToList();
                List<Room> UnregisteredRoom = _context.Rooms.Where(x=>x.BedCount == room && x.OrderRooms.Count == 0).ToList();

                UnregisteredOrderRoom = UnregisteredRoom.Select(x => new OrderRooms {RoomId = x.id }).ToList();
                SelectOrderRoom.AddRange(UnregisteredOrderRoom);
            }
            else
            {
                return RedirectToAction("404 Not found");
            }

            if(SelectOrderRoom.Count == 0)
            {
                return View(roomlistforBedCount);
            }

            foreach (var SelectItem in SelectOrderRoom)
            {
                if ((checkin > SelectItem.CheckOut && checkout > SelectItem.CheckOut) || (checkin < SelectItem.CheckIn && checkout < SelectItem.CheckIn))
                {
                    AvailableRoomsId.Add(SelectItem);
                }
            }


            foreach (var Item in AvailableRoomsId)
            {
                var AvailableRoom = _context.Rooms
                                     .Where(x => x.id == Item.RoomId)
                                     .ToList();
                AvailableRooms.AddRange(AvailableRoom);
            }


            return View(AvailableRooms);

        }

        public IActionResult AllRoom()
        {
            List<Room> rooms = _context.Rooms.ToList();
            return View(rooms);
        }


        public IActionResult RoomDetail(int adults, int childrens, int infants, DateTime checkIn, DateTime checkOut,  int id)
        {
            ViewBag.Checkin = checkIn;
            ViewBag.Checkout = checkOut;
            ViewBag.Adults = adults;
            ViewBag.Childrens = childrens;
            ViewBag.Infants = infants;

                       

                 RoomDetailViewModel RoomDetailMV = new RoomDetailViewModel
            {
                Room = _context.Rooms
                .Include(x => x.RoomAmenities).ThenInclude(x => x.Amenitie)
                .Include(x => x.UserComments).ThenInclude(x => x.AppUser)
                .FirstOrDefault(x => x.id == id),
                Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value),
                Comment = new Comment { RoomId = id }

            };

            RoomDetailMV.AllComment = _context.UserComments.Where(x => x.RoomId == RoomDetailMV.Room.id).ToList();

            int CurrentBedCount = RoomDetailMV.Room.BedCount;
            int CurrentRoomId = RoomDetailMV.Room.id;

            RoomDetailMV.RelatedRoom = _context.Rooms
                .Where(x => x.BedCount == CurrentBedCount && x.id != CurrentRoomId)
                .ToList();

            if (RoomDetailMV.Room == null)
            {
                return RedirectToAction("404 error page");
            }

            return View(RoomDetailMV);
        }


        [HttpPost]
        public IActionResult Comment(Comment comment)
        {
            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if (member == null)
            {
                return RedirectToAction("login", "acocunt");
            }

            RoomDetailViewModel RoomDetailMV = new RoomDetailViewModel
            {
                Room = _context.Rooms
                   .Include(x => x.RoomAmenities).ThenInclude(x => x.Amenitie)
                   .Include(x => x.UserComments).ThenInclude(x => x.AppUser)
                   .FirstOrDefault(x => x.id == comment.RoomId),
                Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value),
                Comment = comment

            };
            RoomDetailMV.AllComment = _context.UserComments.Where(x => x.RoomId == RoomDetailMV.Room.id).ToList();

            int CurrentBedCount = RoomDetailMV.Room.BedCount;
            int CurrentRoomId = RoomDetailMV.Room.id;

            RoomDetailMV.RelatedRoom = _context.Rooms
                .Where(x => x.BedCount == CurrentBedCount && x.id != CurrentRoomId)
                .ToList();

            if (RoomDetailMV.Room == null)
            {
                return RedirectToAction("404 error page");
            }


            if (!ModelState.IsValid)
            {
                return View("roomdetail", RoomDetailMV);
            }

            if (_context.UserComments.Where(x => x.RoomId == RoomDetailMV.Room.id).Any(x => x.AppUserId == member.Id))
            {
                ModelState.AddModelError("", "This user has already commented");
                return View("roomdetail", RoomDetailMV);
            }
            if (string.IsNullOrEmpty(comment.Text))
            {
                return View("roomdetail", RoomDetailMV);

            }

            comment.AppUserId = member.Id;
            comment.Username = member.Fullname;
            comment.Image = member.Image;
            comment.CreatedAt = DateTime.UtcNow.AddHours(4);
            _context.UserComments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("roomdetail", new { id = comment.RoomId });
        }

    }
}
