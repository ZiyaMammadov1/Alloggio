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
    }
}
