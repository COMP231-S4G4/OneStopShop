using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class Cart
    {
       
        private List<CartItem> itemCollection = new List<CartItem>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartItem item = itemCollection
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault();

            if (item == null)
            {
                itemCollection.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                item.Quantity += quantity;
            }

        }

       
    }
}
