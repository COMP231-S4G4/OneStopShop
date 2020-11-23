using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models.ViewModels
{
    public class CartOrder
    {
        public Cart Cart { get; set; }
        public Orders Order { get; set; }
    }
}
