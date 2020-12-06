using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class ViewModel
    {
        public List<Product> product { get; set; }
        public List<Store> store { get; set; }
        public List<Cart> cart { get; set; }
        public Orders order { get; set; }
    }
}