﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OneStopShop.Models;

namespace OneStopShop.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Blogs> Blogs { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<OneStopShop.Models.Orders> Orders { get; set; }

        public DbSet<OneStopShop.Models.Users> Users { get; set; }

        public DbSet<JoinedStore> JoinedStore { get; set; }

        public DbSet<OneStopShop.Models.Reviews> Reviews { get; set; }
    }
}