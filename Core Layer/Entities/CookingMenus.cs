using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core_Layer.Entities
{
    public class CookingMenus
    {
        public int id { get; set; }

        [StringLength(maximumLength:50)]
        public string FoodName { get; set; }

        [StringLength(maximumLength: 150)]
        public string FoodDescription { get; set; }
    }
}
