using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class Subscribers
    {
        [Key]
        public int JoinedStoreId { get; set; }

        public int StoreId { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string email { get; set; }

        public bool IsOwner { get; set; }

        public virtual Store Store { get; set; }

        public virtual Users Users { get; set; }
    }
}