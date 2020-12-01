using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class Subscribers
    {
        [Key]
        public int SubscriberID { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }

        [ForeignKey("UserID")]
        public virtual Users User { get; set; }
    }
}
