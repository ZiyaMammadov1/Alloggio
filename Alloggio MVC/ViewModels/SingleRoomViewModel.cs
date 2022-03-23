using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.ViewModels
{
    public class SingleRoomViewModel
    {
        public RoomDetailViewModel RoomDetailViewModel{ get; set; }
        public OrderRooms BusyTime{ get; set; }
    }
}
