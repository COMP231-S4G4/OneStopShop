using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        public int Quantity { get; set; }

        public int StoreId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        
       
    }
}
