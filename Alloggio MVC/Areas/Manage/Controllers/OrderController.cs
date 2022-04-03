using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Areas.Manage.Controllers
{

    [Area("manage")]

    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly OrderRoomRepository _orderRoomRepository;


        public OrderController(UserManager<AppUser> userManager, OrderRepository orderRepository, OrderRoomRepository orderRoomRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _orderRoomRepository = orderRoomRepository;
        }

        public IActionResult Index()
        {
           
            return View(_orderRepository.GetAll());
        }

        public IActionResult OrderDetail(int id) {
            List<OrderRooms> rooms = _orderRoomRepository.GetAll(x => x.OrderId == id).ToList();
            if(rooms == null)
            {
                return NotFound();
            }

            return View(rooms);
        }

        public IActionResult DeleteOrderRoom(int id)
        {
            var Room = _orderRoomRepository.Get(x => x.id == id);
            var currentOrder = _orderRepository.Get(x=>x.id == Room.OrderId);
            var AllRooms = _orderRoomRepository.GetAll(x=>x.OrderId == Room.OrderId);
            Room.IsDeleted = true;

            foreach (var item in AllRooms)
            {
                if (item.IsDeleted == false)
                {
                    currentOrder.IsDeleted = false;
                    break;
                }
                else
                {
                    currentOrder.IsDeleted = true;

                }
            }

            _orderRoomRepository.Update(Room);
            _orderRepository.Update(currentOrder);
            _orderRoomRepository.Commit();
            _orderRepository.Commit();

            return RedirectToAction("orderdetail", new{id =Room.OrderId});
        }

        public IActionResult DeleteOrder(int id)
        {
            var Order = _orderRepository.Get(x => x.id == id);
            if(Order == null)
            {
                return NotFound();
            }

            Order.IsDeleted = true;

            _orderRepository.Update(Order);
            _orderRepository.Commit();

            var RelatedOrderRoom = _orderRoomRepository.GetAll(x=>x.OrderId == id).ToList();
            if(RelatedOrderRoom == null)
            {
                return NotFound();
            }

            foreach (var item in RelatedOrderRoom)
            {
                item.IsDeleted = true;
                _orderRoomRepository.Update(item);
            }
           
            _orderRoomRepository.Commit();

            return RedirectToAction("index");

        }
   
        public IActionResult EditOrder(int id)
        {
            var order = _orderRepository.Get(x=>x.id == id);
            if(order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult EditOrder (Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }
            Order CurrentOrder = _orderRepository.Get(x=>x.id == order.id);
            if(CurrentOrder == null)
            {
                return NotFound();
            }
            CurrentOrder.FullName = order.FullName;
            CurrentOrder.Email= order.Email;
            CurrentOrder.Phone = order.Phone;
            CurrentOrder.CreateAt = order.CreateAt;
            CurrentOrder.ModifiedAt = DateTime.Now;
            CurrentOrder.TotalPrice = order.TotalPrice;
            CurrentOrder.Note = order.Note;

            _orderRepository.Update(CurrentOrder);
            _orderRepository.Commit();

            return RedirectToAction("index");

        }

        public IActionResult EditOrderRoom (int id)
        {
            var room = _orderRoomRepository.Get(x=>x.id == id);
            if(room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [HttpPost]
        public IActionResult EditOrderRoom(OrderRooms orderroom)
        {

            if (!ModelState.IsValid)
            {
                return View(orderroom);
            }
            var currentOrderRoom = _orderRoomRepository.Get(x=>x.id == orderroom.id);
            if(currentOrderRoom == null)
            {
                return NotFound();
            }
            currentOrderRoom.Count = orderroom.Count;
            currentOrderRoom.Price = orderroom.Price;
            currentOrderRoom.Adult = orderroom.Adult;
            currentOrderRoom.Children = orderroom.Children;
            currentOrderRoom.Infant = orderroom.Infant;
            _orderRoomRepository.Update(currentOrderRoom);
            _orderRoomRepository.Commit();

            return RedirectToAction("index");
        }
    }
}
