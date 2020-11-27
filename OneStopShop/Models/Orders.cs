using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }

        public int? StoreId { get; set; }

        public int? ProductID { get; set; }

        [BindNever]
        public ICollection<CartItem> Lines { get; set; }

        [BindNever]
        public bool? Shipped { get; set; }

       
        public string? CustomerName { get; set; }

       
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }

        
        public string? City { get; set; }

       
        public string? State { get; set; }

        public string? Zip { get; set; }

       
        public string? Country { get; set; }

        public DateTime OrderCreatedDate { get; set; }

        public decimal? TotalCost { get; set; }

        public string? OrderStatus { get; set; }

       





    }
}
