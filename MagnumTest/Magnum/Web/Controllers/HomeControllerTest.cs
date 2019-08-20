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

        [TestCase("", "A", "B", "C", "Name cannot be empty.")]
        [TestCase("A", "", "B", "C", "Subject cannot be empty.")]
        [TestCase("A", "B", "", "C", "Email cannot be empty.")]
        [TestCase("A", "B", "C", "", "Message cannot be empty.")]
        [TestCase("A", "B", "C", "D", null)]
        public void ValidateContactUsFormTest(String name, String subject, String email, String message, string expected)
        {
            MContactUs model = new MContactUs();
            model.Name = name;
            model.Subject = subject;
            model.Email = email;
            model.Message = message;
            string result = controller.ValidateContactUsForm(model);
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual("127.0.0.1", model.IP);
            Assert.AreEqual("Contact", result.ViewName);
            Assert.AreEqual("Thank you for contacting us – we will get back to you soon!", result.ViewData["Message"]);
        }

        [TestCase("", "0000", "1234", "5678")]
        public void SaveContactTestEmpty(String name, String subject, String email, String message)
        {
            MContactUs model = new MContactUs();
            model.Name = name;
            model.Subject = subject;
            model.Email = email;
            model.Message = message;
            ViewResult result = (ViewResult)controller.SaveContactUs(model);
            Assert.AreEqual(null, model.IP);
            Assert.AreEqual("Contact", result.ViewName);
            Assert.AreEqual("Name cannot be empty.", result.ViewData["Message"]);
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