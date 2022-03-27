using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core_Layer.Entities
{
    public class Setting
    {
        public int id { get; set; }

        [StringLength(maximumLength:150)]
        public string Key { get; set; }

        [StringLength(maximumLength: 500)]
        public string Value { get; set; }
    }
}
