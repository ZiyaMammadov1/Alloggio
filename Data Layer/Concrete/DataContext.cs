using Core_Layer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Layer.Concrete
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Amenitie> Amenities { get; set; }
        public DbSet<CookingMenus> CookingMenus { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<AppUser> AppUsers{ get; set; }
        public DbSet<Comment> UserComments { get; set; }
        public DbSet<BedCountForRooms> BedCount { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderRooms> OrderRooms{ get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
    }
}
