using Core_Layer.Entities;
using Data_Layer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Areas.Manage.Helpers.LayoutService
{
    public class AdminLayoutService
    {
        private readonly MessageRepository _messageRepository;

        public AdminLayoutService(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public List<Message> GetMessage()
        {
            return _messageRepository.GetAll(x => x.IsWriting == false).ToList();
        }
    }
}
