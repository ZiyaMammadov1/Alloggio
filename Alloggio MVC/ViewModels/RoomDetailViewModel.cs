using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.ViewModels
{
    public class RoomDetailViewModel
    {
        public Room  Room { get; set; }
        public List<Room> RelatedRoom { get; set; }
        public Dictionary<string,string> Settings { get; set; }
        public Comment Comment { get; set; }
        public List<Comment> AllComment { get; set; }
    }
}
