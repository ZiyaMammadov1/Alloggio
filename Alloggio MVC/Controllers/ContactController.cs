using Alloggio_MVC.ViewModels;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Controllers
{
    public class ContactController : Controller
    {
        private readonly DataContext _context;
        private readonly MessageRepository _messageRepository;
        public ContactController(DataContext context, MessageRepository messageRepository)
        {
            _context = context;
            _messageRepository = messageRepository;
        }
        public IActionResult Index()
        {
            ContactViewModel ContactVm = new ContactViewModel
            {
                Setting = _context.Settings.ToDictionary(x => x.Key, x => x.Value),
                Message = new Message()
            };

            return View(ContactVm);
        }

        [HttpPost]
        public IActionResult Index(ContactViewModel ContactVm)
        {
            ContactVm.Setting = _context.Settings.ToDictionary(x => x.Key, x => x.Value);
            if (!ModelState.IsValid)
            {
                return View(ContactVm);
            }

            Message NewMessage = new Message()
            {
                FullName = ContactVm.Message.FullName,
                Email = ContactVm.Message.Email,
                Content = ContactVm.Message.Content,
                IsWriting = false,
            };

            _messageRepository.Add(NewMessage);
            _messageRepository.Commit();

            return RedirectToAction("index","home");
        }
    }
}
