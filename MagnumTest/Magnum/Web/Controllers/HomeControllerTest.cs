using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Magnum.Web.Models;
using Magnum.Api.Caches;
using Magnum.Api.Models;
using System.Collections.Generic;
using System.Collections;

namespace Magnum.Web.Controllers
{
    public class HomeControllerTest
    {
        HomeController controller;

        [SetUp]
        public void Setup()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var mockController = new Mock<HomeController>() { CallBase = true };

            var iCacheMock = new Mock<ICache>();
            mockController.Setup(foo => foo.GetContentCache()).Returns(iCacheMock.Object);
            mockController.Setup(foo => foo.GetProductTypeCache()).Returns(iCacheMock.Object);
            mockController.Setup(foo => foo.GetProductsCache()).Returns(iCacheMock.Object);

            MContent bestSellers = new MContent();
            bestSellers.Values = new Dictionary<string, string>();
            bestSellers.Values.Add("b1", "ITEM-001");
            bestSellers.Values.Add("b2", "ITEM-002");
            iCacheMock.Setup(foo => foo.GetValue("code/Best_Seller_Products")).Returns(bestSellers);

            MContent newArrivals = new MContent();
            newArrivals.Values = new Dictionary<string, string>();
            newArrivals.Values.Add("n1", "ITEM-003");
            newArrivals.Values.Add("n2", "ITEM-004");
            newArrivals.Values.Add("n3", "ITEM-005");
            iCacheMock.Setup(foo => foo.GetValue("code/New_Arrival_Products")).Returns(newArrivals);

            controller = mockController.Object;
            controller.ControllerContext = controllerContext;
        }

        [Test]
        public void PrivacyTest()
        {
            ViewResult result = (ViewResult)controller.Privacy();
            Assert.IsNotNull(result);
        }

        [Test]
        public void AboutTest()
        {
            ViewResult result = (ViewResult)controller.About();
            Assert.IsNotNull(result);
        }

        [Test]
        public void IndexTest()
        {
            ViewResult result = (ViewResult)controller.Index();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, ((ArrayList)result.ViewData["BestSellerList"]).Count);
            Assert.AreEqual(3, ((ArrayList)result.ViewData["NewArrivalList"]).Count);
            Assert.AreEqual(5, ((ArrayList)result.ViewData["ProductList"]).Count);
        }

        [Test]
        public void ErrorTest()
        {
            ViewResult result = (ViewResult)controller.Error();
            ErrorViewModel evm = (ErrorViewModel)result.Model;
            Assert.IsNotNull(evm);
        }

        [Test]
        public void GetContentCache()
        {
            var cache = new HomeController().GetContentCache();
            Assert.NotNull(cache);
        }
    }
}