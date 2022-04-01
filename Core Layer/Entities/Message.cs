using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core_Layer.Entities
{
    public class Message
    {
        public int id { get; set; }

        [Required]
        [StringLength(maximumLength:50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 300)]
        public string Content { get; set; }

        public bool IsWriting { get; set; }
    }
}
