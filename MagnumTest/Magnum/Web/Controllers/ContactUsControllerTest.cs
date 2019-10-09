using System;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;

using Its.Onix.Core.Business;
using Its.Onix.Core.Caches;
using Its.Onix.Core.Smtp;
using Its.Onix.Erp.Models;
using Its.Onix.Erp.Businesses.ContactUs;

namespace Magnum.Web.Controllers
{
    public class ContactUsControllerTest : BaseTest
    {
        public ContactUsControllerTest() : base()
        {
        }

        ContactUsController controller;
        Mock<ContactUsController> mockController;
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

            mockController = new Mock<ContactUsController>() { CallBase = true };
            var mockOpr = new Mock<IBusinessOperationManipulate<MContactUs>>();
            mockContactUsOpr = mockOpr.Object;
            mockOpr.Setup(foo => foo.Apply(It.IsAny<MContactUs>())).Returns(0);

            mockController.Setup(foo => foo.GetSaveContactUsOperation()).Returns(mockContactUsOpr);
            var iCacheMock = new Mock<ICacheContext>();
            mockController.Setup(foo => foo.GetContentCache()).Returns(iCacheMock.Object);

            var mockSmtp = new Mock<ISmtpContext>();
            mockController.Setup(foo => foo.GetSmtpContext()).Returns(mockSmtp.Object);

            controller = mockController.Object;
            controller.ControllerContext = controllerContext;
        }

        [Test]
        public void ContactTest()
        {
            ViewResult result = (ViewResult)controller.Contact();
            Assert.IsNotNull(result);
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
            Assert.AreEqual("Your message has been received and we will contact you soon.", result.ViewData["Message"]);
        }

        [TestCase("Maxnum", "0000", "1234", "5678")]
        public void SaveContactFail(String name, String subject, String email, String message)
        {
            MContactUs model = new MContactUs();
            model.Name = name;
            model.Subject = subject;
            model.Email = email;
            model.Message = message;
            mockController.Setup(foo => foo.SendEmail(It.IsAny<MContactUs>())).Returns(false);
            ViewResult result = (ViewResult)controller.SaveContactUs(model);
            Assert.AreEqual("127.0.0.1", model.IP);
            Assert.AreEqual("Contact", result.ViewName);
            Assert.AreEqual("Unable to send the message, internal server error.", result.ViewData["Message"]);
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
            var realController = new ContactUsController();
            SaveContactUs result = (SaveContactUs)realController.GetSaveContactUsOperation();
            Assert.IsNotNull(result);
        }

        [Test]
        public void SendEmailNoEnvironment()
        {
            string nullString = null;
            var mockController = new Mock<ContactUsController>() { CallBase = true };
            mockController.Setup(foo => foo.GetEmailTo()).Returns(nullString);

            MContactUs contactUs = new MContactUs();
            bool result = mockController.Object.SendEmail(contactUs);
            Assert.IsFalse(result);
        }

        [Test]
        public void GetContentCache()
        {
            var cache = new ContactUsController().GetContentCache();
            Assert.NotNull(cache);
        }
    }
}