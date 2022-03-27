using Core_Layers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core_Layer.Entities
{
    public class Order
    {
        public int id { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(maximumLength:50)]
        public string FullName { get; set; }


        [Required]
        [StringLength(maximumLength: 50)]
        public string Phone { get; set; }


        [Required]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }

       
        
        [StringLength(maximumLength: 500)]
        public string Note { get; set; }

        public DateTime CreateAt{ get; set; }
        public DateTime ModifiedAt { get; set; }
        public string AppUserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        [Column(TypeName="decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public List<OrderRooms> OrderRooms { get; set; }
        public AppUser AppUser { get; set; }
    }
}
