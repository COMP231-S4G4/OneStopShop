using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models.ViewModels
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }

        public IEnumerable<Product> Product { get; set; }

        public Orders order { get; set; }
    }
}