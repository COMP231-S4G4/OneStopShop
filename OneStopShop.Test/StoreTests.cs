using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using OneStopShop.Controllers;
using OneStopShop.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace OneStopShop.Test
{
    public class StoreTests
    {

        List<Store> entities;
        DbContextOptions<ApplicationDbContext> options;
        Mock<IWebHostEnvironment> mockEnvironment = new Mock<IWebHostEnvironment>();
        Mock<IHttpContextAccessor> mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        StoresController storeController;
        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "OneStopShopDatabase")
            .Options;

            //...Setup the mock as needed
            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");

            entities = new List<Store>();

            using (var context = new ApplicationDbContext(options))
            {
                var dataProtectionProvider = new EphemeralDataProtectionProvider();
                var blogsController = new BlogsController(context, dataProtectionProvider, mockHttpContextAccessor.Object, mockEnvironment.Object);

                if (context.Stores == null || context.Stores.CountAsync().Result == 0)
                {
                    context.Stores.Add(new Store
                    {
                        StoreId=1,
                        Email = "test@mailinator.com",
                        PhoneNumber = "8585858585",
                        SellerFirstname = "Rob",
                        SellerLasttname = "Miller",
                        StoreName = "UE Enterprise",
                        StoreDescription = "This is description of store"
                    });
                    context.SaveChanges();
                }
                if (context.Products == null || context.Products.CountAsync().Result == 0)
                {
                    context.Products.Add(new Product
                    {
                        ProductColor = "#FFFFFF",
                        ProductID = 1,
                        ProductImage = "",
                        IsAddedToCart = false,
                        ProductCreatedDate = DateTime.Now,
                        ProductDescription = "The aluminium case is lightweight and made from 100 per cent recycled aerospace-grade alloy."
                                       + "The Sport Band is made from a durable yet surprisingly soft high - performance fluoroelastomer"
                                       + "with an innovative pin - and - tuck closure.",
                        ProductName = "Apple smart watch",
                        ProductPrice = 5873,
                        ProductSize = "1",
                        StoreId = 1,
                        store = new Store()
                        {
                            Email = "test@mailinator.com",
                            PhoneNumber = "8585858585",
                            SellerFirstname = "Rob",
                            SellerLasttname = "Miller",
                            StoreName = "UE Enterprise",
                            StoreDescription = "This is description of store",
                            StoreId = 1
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Test Case for find products from StoreID
        /// </summary>
        [Test]
        public void Index_GetStores()
        {

            using (var context = new ApplicationDbContext(options))
            {
                var dataProtectionProvider = new EphemeralDataProtectionProvider();
                storeController = new StoresController(context, dataProtectionProvider, mockHttpContextAccessor.Object, mockEnvironment.Object);
                var resultTask = storeController.Index();
                resultTask.Wait();
                var model = (List<Store>)((ViewResult)resultTask.Result).Model;
                Assert.AreEqual(1, model.Count);
            }
        }
        
        /// <summary>
        /// Test Case get product details from productID
        /// </summary>
        [Test]
        public void Details_By_StoreId()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var dataProtectionProvider = new EphemeralDataProtectionProvider();
                storeController = new StoresController(context, dataProtectionProvider, mockHttpContextAccessor.Object, mockEnvironment.Object);
                var resultTask = storeController.Details(1);
                resultTask.Wait();
                Assert.IsNotNull(resultTask.Result);
                var model = (Tuple<IList<Product>, Store>)((ViewResult)resultTask.Result).Model;
                Assert.AreEqual(1, model.Item2.StoreId);
            }
        }
      

    }
}
