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

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int StoreId { get; set; }

        public DateTime OrderCreatedDate { get; set; }

        public DateTime OrderModifiedDate { get; set; }
    }
}
