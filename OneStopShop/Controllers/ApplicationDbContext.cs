using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OneStopShop.Models;

namespace OneStopShop.Models
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// This class links the database server to the data model classes in the application
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Blogs> Blogs { get; set; }
        public DbSet<OneStopShop.Models.Orders> Orders { get; set; }
        public DbSet<OneStopShop.Models.Users> Users { get; set; }
        public DbSet<Subscribers> JoinedStore { get; set; }
        public DbSet<OneStopShop.Models.Reviews> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CustomOrders> CustomOrders { get; set; }
        public DbSet<Subscribers> Subscribers { get; set; }
    }
}