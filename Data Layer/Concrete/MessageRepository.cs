using Core_Layer.Abstract;
using Core_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Layer.Concrete
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(DataContext context) : base(context)
        {

        }
    }
}
