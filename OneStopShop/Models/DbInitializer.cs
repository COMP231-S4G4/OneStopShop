using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneStopShop.Models
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Stores.Any())
            {
                return;
            }
            Store[] stores = new Store[]
            {
                new Store
                {
                    StoreName="ABC",
                    SellerFirstname="Nick",
                    SellerLasttname="Gill",
                    StoreDescription="It is a designer store",
                    PhoneNumber="647667666",
                    Email="nick@gill.com",

                },
                new Store
                {
                    StoreName="ABB",
                    SellerFirstname="Amanda",
                    SellerLasttname="Cory",
                    StoreDescription="It is a designer store",
                    PhoneNumber="647667669",
                    Email="amanda@gill.com",

                },
                new Store
                {
                    StoreName="GrandStore",
                    SellerFirstname="John",
                    SellerLasttname="Smith",
                    StoreDescription="It is a designer store",
                    PhoneNumber="647667660",
                    Email="john@gill.com",

                }
            };
            context.Stores.AddRange(stores);

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        ProductName = "pant",
                        ProductDescription = "A blue pant",
                        ProductColor = "blue",
                        ProductPrice = 275,
                        ProductCreatedDate = new DateTime(2020, 11, 10, 10, 30, 45),
                        ProductModifiedDate = new DateTime(2015, 11, 10, 10, 30, 45),
                        ProductID = 1,
                        ProductImage = "",                       
                        ProductSize = "Medium"

                    },
                    new Product
                    {
                        ProductName = "shirt",
                        ProductDescription = "A blue shirt",
                        ProductColor = "blue",
                        ProductPrice = 275,
                        ProductCreatedDate = new DateTime(2020, 11, 10, 10, 30, 45),
                        ProductModifiedDate = new DateTime(2015, 11, 10, 10, 30, 45),
                        ProductID = 1,
                        ProductImage = "",
                        ProductSize = "Medium"
                    },
                      new Product
                      {
                          ProductName = "shirt",
                          ProductDescription = "A blue shirt",
                          ProductColor = "blue",
                          ProductPrice = 275,
                          ProductCreatedDate = new DateTime(2020, 11, 10, 10, 30, 45),
                          ProductModifiedDate = new DateTime(2015, 11, 10, 10, 30, 45),
                          ProductID = 1,
                          ProductImage = "",
                          ProductSize = "Medium"
                      }
                      );
                context.SaveChanges();

            }

        }
    }
}

