using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public class Cart
    {
       
        private List<CartItem> itemCollection = new List<CartItem>();

        public virtual void AddItem(Product product, int quantity,int storeid)
        {
            CartItem item = itemCollection
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault();

            if (item == null)
            {
                itemCollection.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    StoreId= storeid
                });
            }
            else
            {
                item.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Product product) =>
           itemCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);

        public decimal ComputeTotalValue() =>
            itemCollection.Sum(e => e.Product.ProductPrice * e.Quantity);

        public virtual void Clear() => itemCollection.Clear();
        public IEnumerable<CartItem> Lines => itemCollection;


    }
}
