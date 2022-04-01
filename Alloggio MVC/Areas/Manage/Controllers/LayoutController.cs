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
    public class LayoutController : Controller
    {
        private readonly MessageRepository _messageRepository;

        public LayoutController(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public IActionResult MessageSetIsWriting(int id)
        {
           var message = _messageRepository.Get(x=>x.id == id);

            if(message == null)
            {
                return NotFound();
            }
            else
            {
                message.IsWriting = true;
                _messageRepository.Update(message);
                _messageRepository.Commit();
            }
            return RedirectToAction("index","dashboard","manage");
        }
    }
}
