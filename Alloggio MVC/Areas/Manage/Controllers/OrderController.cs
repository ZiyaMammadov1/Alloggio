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

            return View(_orderRepository.GetAll(x=>x.IsDeleted == false).ToList());
        }

        public IActionResult OrderDetail(int id)
        {
            List<OrderRooms> rooms = _orderRoomRepository.GetAll(x => x.OrderId == id).ToList();
            if (rooms == null)
            {
                return NotFound();
            }

            return View(rooms);
        }

        public IActionResult DeleteOrderRoom(int id)
        {
            var Room = _orderRoomRepository.Get(x => x.id == id);
            var currentOrder = _orderRepository.Get(x => x.id == Room.OrderId);
            var AllRooms = _orderRoomRepository.GetAll(x => x.OrderId == Room.OrderId);
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

            return RedirectToAction("orderdetail", new { id = Room.OrderId });
        }

        public IActionResult DeleteOrder(int id)
        {
            var Order = _orderRepository.Get(x => x.id == id);
            if (Order == null)
            {
                return NotFound();
            }

            Order.IsDeleted = true;

            _orderRepository.Update(Order);
            _orderRepository.Commit();

            var RelatedOrderRoom = _orderRoomRepository.GetAll(x => x.OrderId == id).ToList();
            if (RelatedOrderRoom == null)
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
            var order = _orderRepository.Get(x => x.id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult EditOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }
            Order CurrentOrder = _orderRepository.Get(x => x.id == order.id);
            if (CurrentOrder == null)
            {
                return NotFound();
            }
            CurrentOrder.FullName = order.FullName;
            CurrentOrder.Email = order.Email;
            CurrentOrder.Phone = order.Phone;
            CurrentOrder.CreateAt = order.CreateAt;
            CurrentOrder.ModifiedAt = DateTime.Now;
            CurrentOrder.TotalPrice = order.TotalPrice;
            CurrentOrder.Note = order.Note;

            _orderRepository.Update(CurrentOrder);
            _orderRepository.Commit();

            return RedirectToAction("index");

        }

        public IActionResult EditOrderRoom(int id)
        {
            var room = _orderRoomRepository.Get(x => x.id == id);
            if (room == null)
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
            var currentOrderRoom = _orderRoomRepository.Get(x => x.id == orderroom.id);
            if (currentOrderRoom == null)
            {
                return NotFound();
            }
            currentOrderRoom.Count = orderroom.Count;
            currentOrderRoom.Price = orderroom.Price;
            currentOrderRoom.Adult = orderroom.Adult;
            currentOrderRoom.Children = orderroom.Children;
            currentOrderRoom.Infant = orderroom.Infant;

            List<OrderRooms> BusyDate = _orderRoomRepository.GetAll(x => x.RoomId == orderroom.RoomId && x.IsDeleted == false).ToList();
            BusyDate.Remove(currentOrderRoom);

            DateTime date = new DateTime(orderroom.CheckIn.Year, orderroom.CheckIn.Month, 1);


            if (orderroom.CheckIn.Month == currentOrderRoom.CheckIn.Month && orderroom.CheckOut.Month == currentOrderRoom.CheckIn.Month)
            {
                BusyDate = BusyDate.Where(x => x.CheckIn.Month == currentOrderRoom.CheckIn.Month || x.CheckOut.Month == currentOrderRoom.CheckOut.Month).ToList();
                if (!(orderroom.CheckIn.Month >= DateTime.Now.Month) && !(orderroom.CheckOut.Month >= DateTime.Now.Month))
                {
                    ModelState.AddModelError("", "CheckIn or CheckOut does not exist");
                    return View(orderroom);
                }

                if (orderroom.CheckIn.Day > orderroom.CheckOut.Day)
                {
                    ModelState.AddModelError("", "CheckIn does not greater than CheckOut");
                    return View(orderroom);
                }
                int MonthDayCount;
                List<int> FreeMonthList = new List<int>();
                do
                {
                    FreeMonthList.Add(date.Day);
                    date = date.AddDays(1);
                } while (date.Month == orderroom.CheckIn.Month);
                MonthDayCount = FreeMonthList.Count();

                foreach (var Date in BusyDate)
                {
                    if (Date.CheckIn.Month < currentOrderRoom.CheckIn.Month)
                    {
                        for (int i = 1; i <= Date.CheckOut.Day; i++)
                        {
                            FreeMonthList.Remove(i);
                        }

                    }
                    else if (Date.CheckIn.Month > currentOrderRoom.CheckIn.Month)
                    {

                        for (int i = Date.CheckIn.Day; i <= MonthDayCount; i++)
                        {
                            FreeMonthList.Remove(i);
                        }
                    }
                    else
                    {
                        for (int i = Date.CheckIn.Day; i <= Date.CheckOut.Day; i++)
                        {
                            FreeMonthList.Remove(i);
                        }
                    }

                }
                if (!FreeMonthList.Contains(orderroom.CheckIn.Day) || !FreeMonthList.Contains(orderroom.CheckOut.Day))
                {
                    ModelState.AddModelError("", "CheckIn or CheckOut dates conflict with other dates");
                    return View(orderroom);
                }

                currentOrderRoom.CheckIn = orderroom.CheckIn;
                currentOrderRoom.CheckOut = orderroom.CheckOut;

            }
            else
            {
                if (orderroom.CheckIn.Month > orderroom.CheckOut.Month)
                {
                    ModelState.AddModelError("", "CheckIn does not greater than CheckOut");
                    return View(orderroom);
                }


                int LastMonthDayCount;
                int FirstMonthDayCount;

                List<int> CheckInMonthList = new List<int>();
                do
                {
                    CheckInMonthList.Add(date.Day);
                    date = date.AddDays(1);
                } while (date.Month == orderroom.CheckIn.Month);
                FirstMonthDayCount = CheckInMonthList.Count;



                List<int> CheckOutMonthList = new List<int>();
                do
                {
                    CheckOutMonthList.Add(date.Day);
                    date = date.AddDays(1);
                } while (date.Month == orderroom.CheckOut.Month);
                LastMonthDayCount = CheckOutMonthList.Count;


                foreach (var Date in BusyDate)
                {
                    if (Date.CheckIn.Month == Date.CheckOut.Month && Date.CheckIn.Month == orderroom.CheckIn.Month)
                    {
                        for (int i = Date.CheckIn.Day; i <= Date.CheckOut.Day; i++)
                        {
                            CheckInMonthList.Remove(i);
                        }
                    }
                    else if (Date.CheckIn.Month == Date.CheckOut.Month && Date.CheckIn.Month != orderroom.CheckIn.Month)
                    {
                        for (int i = Date.CheckIn.Day; i <= Date.CheckOut.Day; i++)
                        {
                            CheckOutMonthList.Remove(i);
                        }
                    }
                    else
                    {

                        for (int i = Date.CheckIn.Day; i <= LastMonthDayCount; i++)
                        {
                            CheckInMonthList.Remove(i);
                        }
                        for (int i = 1; i <= Date.CheckOut.Day; i++)
                        {
                            CheckOutMonthList.Remove(i);
                        }
                    }
                }


                if (orderroom.CheckIn.Month == orderroom.CheckOut.Month)
                {
                    for (int i = orderroom.CheckIn.Day; i <= orderroom.CheckOut.Day; i++)
                    {
                        if (!CheckInMonthList.Contains(i))
                        {
                            ModelState.AddModelError("", "CheckIn conflict other order");
                            return View(orderroom);
                        }
                    }
                }
                else
                {
                    for (int i = orderroom.CheckIn.Day; i <= FirstMonthDayCount; i++)
                    {
                        if (!CheckInMonthList.Contains(i))
                        {
                            ModelState.AddModelError("", "CheckIn conflict other order");
                            return View(orderroom);
                        }
                    }
                    for (int i = 1; i <= orderroom.CheckOut.Day; i++)
                    {
                        if (!CheckOutMonthList.Contains(i))
                        {
                            ModelState.AddModelError("", "CheckOut conflict other order");
                            return View(orderroom);
                        }
                    }
                }


                currentOrderRoom.CheckIn = orderroom.CheckIn;
                currentOrderRoom.CheckOut = orderroom.CheckOut;
            }






            _orderRoomRepository.Update(currentOrderRoom);
            _orderRoomRepository.Commit();

            return RedirectToAction("index");
        }
    }
}
