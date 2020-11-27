using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

       public int ProductId { get; set; }

        public int StoreId { get; set; }

        public int Quantity { get; set; }

        public decimal Cost { get; set; }
    }
}
