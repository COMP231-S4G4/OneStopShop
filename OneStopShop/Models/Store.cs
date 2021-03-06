﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        public string StoreName { get; set; }
        public string SellerFirstname { get; set; }
        public string SellerLasttname { get; set; }
        public string StoreDescription { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Product> product { get; set; }
        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<Subscribers> JoinedStore { get; set; }
    }
}