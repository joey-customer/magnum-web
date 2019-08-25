using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Magnum.Web.Models;
using Magnum.Api.Caches;

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
        }

        [Test]
        public void ProductsTest()
        {
            ViewResult result = (ViewResult)controller.Products();
            Assert.IsNotNull(result);
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