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
    public class CookingController : Controller
    {
        private readonly CookingRepository _cookingRepository;
        public CookingController(CookingRepository cookingRepository)
        {
            _cookingRepository = cookingRepository;
        }
        public IActionResult Index()
        {
            return View(_cookingRepository.GetAll());
        }
    }
}
