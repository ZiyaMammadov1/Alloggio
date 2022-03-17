using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.ViewModels
{
    public class HomeViewModels
    {
        public List<Slider> Sliders { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Service> Services { get; set; }
        public List<Testimonial> Testimonials{ get; set; }
        public Dictionary<string, string> Settings { get; set; }
        public Checking MainChecking { get; set; }
        public string SubscriptionEmail { get; set; }

    }
}
