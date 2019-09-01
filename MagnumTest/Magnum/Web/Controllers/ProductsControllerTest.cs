using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Magnum.Api.Caches;
using Magnum.Api.Models;
using System.Collections.Generic;

namespace Magnum.Web.Controllers
{
    public class ProductsControllerTest
    {
        ProductsController controller;

        [SetUp]
        public void Setup()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var mockController = new Mock<ProductsController>() { CallBase = true };

            var iCacheMock = new Mock<ICache>();
            mockController.Setup(foo => foo.GetContentCache()).Returns(iCacheMock.Object);
            mockController.Setup(foo => foo.GetProductTypeCache()).Returns(iCacheMock.Object);
            mockController.Setup(foo => foo.GetProductsCache()).Returns(iCacheMock.Object);

            Dictionary<string, BaseModel> products = new Dictionary<string, BaseModel>();
            MProduct product1 = new MProduct();
            product1.ProductType = "";
            products.Add("ITEM-001", product1);
            iCacheMock.Setup(foo => foo.GetValues()).Returns(products);

            controller = mockController.Object;
            controller.ControllerContext = controllerContext;
        }

        [Test]
        public void ProductsTest()
        {
            ViewResult result = (ViewResult)controller.Products();
            Assert.IsNotNull(result);
        }
    }
}