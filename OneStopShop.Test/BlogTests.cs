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
    public class BlogTests
    {

        List<Blogs> entities;
        DbContextOptions<ApplicationDbContext> options;
        Mock<IWebHostEnvironment> mockEnvironment = new Mock<IWebHostEnvironment>();
        Mock<IHttpContextAccessor> mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        BlogsController blogsController;
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

            entities = new List<Blogs>();

            using (var context = new ApplicationDbContext(options))
            {
                var dataProtectionProvider = new EphemeralDataProtectionProvider();
                var blogsController = new BlogsController(context, dataProtectionProvider, mockHttpContextAccessor.Object, mockEnvironment.Object);

                if (context.Blogs == null || context.Blogs.CountAsync().Result == 0)
                {
                    context.Blogs.Add(new Blogs
                    {
                        BlogId = 1,
                        BlogTitle = "Title File",
                        BlogCreatedDate = DateTime.Now,
                        BlogDescription = "blog Description",
                        StoreId = 1
                    });
                    context.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Test Case for find products from StoreID
        /// </summary>
        [Test]
        public void Index_By_StoredId()
        {

            using (var context = new ApplicationDbContext(options))
            {
                var dataProtectionProvider = new EphemeralDataProtectionProvider();
                blogsController = new BlogsController(context, dataProtectionProvider, mockHttpContextAccessor.Object, mockEnvironment.Object);
                var resultTask = blogsController.Index(1);
                resultTask.Wait();
                var model = (List<Blogs>)((ViewResult)resultTask.Result).Model;
                Assert.IsNotNull(resultTask.Result);
                Assert.AreEqual(1, model.Count);
            }
        }
        /// <summary>
        /// Test case for not found any record without passing storeID 
        /// </summary>
        [Test]
        public void Index_NotFound_By_StoreId()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var dataProtectionProvider = new EphemeralDataProtectionProvider();
                blogsController = new BlogsController(context, dataProtectionProvider, mockHttpContextAccessor.Object, mockEnvironment.Object);
                var resultTask = blogsController.Index();
                resultTask.Wait();
                var response = resultTask.Result as NotFoundResult;
                Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
            }
        }
        /// <summary>
        /// Test Case get product details from blogID
        /// </summary>
        [Test]
        public void Details_By_BlogId()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var dataProtectionProvider = new EphemeralDataProtectionProvider();
                blogsController = new BlogsController(context, dataProtectionProvider, mockHttpContextAccessor.Object, mockEnvironment.Object);
                var resultTask = blogsController.Details(1);
                resultTask.Wait();
                Assert.IsNotNull(resultTask.Result);
                var model = (Blogs)((ViewResult)resultTask.Result).Model;
                Assert.AreEqual(1, model.BlogId);
            }
        }
        /// <summary>
        /// Test case for blog not found when blogID empty
        /// </summary>
        [Test]
        public void Details_NotFound_By_BlogId()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var dataProtectionProvider = new EphemeralDataProtectionProvider();
                blogsController = new BlogsController(context, dataProtectionProvider, mockHttpContextAccessor.Object, mockEnvironment.Object);
                var resultTask = blogsController.Details(0);
                resultTask.Wait();
                var response = resultTask.Result as NotFoundResult;
                Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
            }
        }
        [Test]
        public void Product_NotFound_By_BlogId()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var dataProtectionProvider = new EphemeralDataProtectionProvider();
                blogsController = new BlogsController(context, dataProtectionProvider, mockHttpContextAccessor.Object, mockEnvironment.Object);
                var resultTask = blogsController.Details(3);
                resultTask.Wait();
                var response = resultTask.Result as NotFoundResult;
                Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
