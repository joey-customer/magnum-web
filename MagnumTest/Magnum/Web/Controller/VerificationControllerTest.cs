using System;
using MagnumWeb.Controllers;
using NUnit.Framework;
using Moq;
using Magnum.Api.Businesses.Registrations;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Magnum.Consoles
{
    public class VerificationControllerTest
    {
        VerificationController controller;
        Mock<IBusinessOperationManipulate<MRegistration>> mockOpr;


        [SetUp]
        public void Setup()
        {
            var mockController = new Mock<VerificationController>() { CallBase = true };
            mockController.Setup(foo => foo.GetRemoteIP()).Returns("172.0.0.1");
            controller = mockController.Object;

            mockOpr = new Mock<IBusinessOperationManipulate<MRegistration>>();
            controller.Opr = mockOpr.Object;
        }

        [TestCase("Maxnum", "0000", "1234", "5678")]
        public void RegisterSuccess(String product, String group, String serial, String pin)
        {
            mockOpr.Setup(foo => foo.Apply(It.IsAny<MRegistration>())).Returns(0);

            ViewResult result = (ViewResult)controller.Check(product, group, serial, pin);

            List<object> keys = new List<object>(result.ViewData.Keys);
            List<object> values = new List<object>(result.ViewData.Values);
            Assert.AreEqual(keys[0], "Serial");
            Assert.AreEqual(keys[1], "PIN");
            Assert.AreEqual(values[0], serial);
            Assert.AreEqual(values[1], pin);
            Assert.AreEqual(result.ViewName, "Success");
        }

        [TestCase("Maxnum", "0000", "1234", "5678")]
        public void RegisterFail(String product, String group, String serial, String pin)
        {
            mockOpr.Setup(foo => foo.Apply(It.IsAny<MRegistration>())).Throws(new Exception("Invalid barcode"));

            ViewResult result = (ViewResult)controller.Check(product, group, serial, pin);

            List<object> keys = new List<object>(result.ViewData.Keys);
            List<object> values = new List<object>(result.ViewData.Values);
            Assert.AreEqual(keys[0], "Message");
            Assert.AreEqual(values[0], "Invalid barcode");
            Assert.AreEqual(result.ViewName, "Fail");

        }
    }
}