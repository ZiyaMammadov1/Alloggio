using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Layer.Entities
{
    public class Blog
    {
        public int id { get; set; }
        public string Image { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
