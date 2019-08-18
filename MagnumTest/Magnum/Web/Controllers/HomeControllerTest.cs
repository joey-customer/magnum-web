using System;
using NUnit.Framework;
using Moq;
using Magnum.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using Magnum.Api.Commons.Business;
using Magnum.Api.Businesses.ContactUs;
using Magnum.Web.Models;

namespace Magnum.Web.Controllers
{
    public class HomeControllerTest
    {
        HomeController controller;
        IBusinessOperationManipulate<MContactUs> mockContactUsOpr;


        [SetUp]
        public void Setup()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            controllerContext.HttpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");

            var mockController = new Mock<HomeController>() { CallBase = true };
            var mockOpr = new Mock<IBusinessOperationManipulate<MContactUs>>();
            mockContactUsOpr = mockOpr.Object;
            mockOpr.Setup(foo => foo.Apply(It.IsAny<MContactUs>())).Returns(0);

            mockController.Setup(foo => foo.GetSaveContactUsOperation()).Returns(mockContactUsOpr);
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
        public void ContactTest()
        {
            ViewResult result = (ViewResult)controller.Contact();
            Assert.IsNotNull(result);
        }

        [Test]
        public void ErrorTest()
        {
            ViewResult result = (ViewResult)controller.Error();
            ErrorViewModel evm = (ErrorViewModel)result.Model;
            Assert.IsNotNull(evm);
        }

        [TestCase("Maxnum", "0000", "1234", "5678")]
        public void SaveContactTest(String name, String subject, String email, String message)
        {
            MContactUs model = new MContactUs();
            model.Name = name;
            model.Subject = subject;
            model.Email = email;
            model.Message = message;
            ViewResult result = (ViewResult)controller.SaveContactUs(model);
            Assert.AreEqual(model.IP, "127.0.0.1");
            Assert.AreEqual(result.ViewName, "Contact");
        }

        [Test]
        public void GetSaveContactUsOperationTest()
        {
            var mockController = new Mock<HomeController>() { CallBase = true };
            controller = mockController.Object;

            SaveContactUs result = (SaveContactUs)controller.GetSaveContactUsOperation();
            Assert.IsNotNull(result);
        }
    }
}