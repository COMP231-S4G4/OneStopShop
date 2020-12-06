using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class Wishlist
    {

        [Key]
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public bool IsAddedToWishlist { get; set; }
        public virtual Product Product { get; set; }
    }
}
