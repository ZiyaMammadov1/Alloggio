using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core_Layer.Entities
{
    public class Message
    {
        public int id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
    }
}
