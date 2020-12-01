using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class JoinedStore
    {
        [Key]
        public int JoinedStoreId { get; set; }

        public int StoreId { get; set; }

        public int UserId { get; set; }

        public bool IsOwner { get; set; }

        public virtual Store Store { get; set; }

        public virtual Users Users { get; set; }
    }
}