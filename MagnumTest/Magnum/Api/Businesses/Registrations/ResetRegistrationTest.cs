using System;
using NUnit.Framework;
using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Registrations
{
    public class ResetRegistrationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("9999999999", "ABCDEFGHIJKLM")]
        public void ResetRegistrationWithCodeNotEmptyTest(string serial, string pin)
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            var opt = (IBusinessOperationManipulate<MRegistration>)FactoryBusinessOperation.CreateBusinessOperationObject("ResetRegistration");

            MRegistration rg = new MRegistration();
            rg.Pin = pin;
            rg.SerialNumber = serial;

            try
            {
                opt.Apply(rg);
                Assert.Fail("Exception should be thrown because of barcode not found!!!");
            }
            catch (Exception)
            {
                //Do nothing
            }
        }

        [TestCase("", "")]
        [TestCase("", "AAAAAAAAAA")]
        [TestCase("AAAAAAAA", "")]
        public void CreateRegistrationWithEmptyTest(string serial, string pin)
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            var opt = (IBusinessOperationManipulate<MRegistration>)FactoryBusinessOperation.CreateBusinessOperationObject("ResetRegistration");

            MRegistration rg = new MRegistration();
            rg.Pin = pin;
            rg.SerialNumber = serial;

            try
            {
                opt.Apply(rg);
                Assert.Fail("Exception should be thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("SerialNumber, PIN must not be null!!!", e.Message);
            }
        }

        [TestCase(false, null, "Serial number and PIN not found")]
        [TestCase(true, false, "Serial number and PIN has not been registered yet")]
        [TestCase(true, true, "")]
        public void ResetRegistrationWithCodeNotFoundTest(bool barcodeFound, bool isActivated, string keyword)
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            if (barcodeFound)
            {
                MBarcode bc = new MBarcode();
                bc.IsActivated = isActivated;
                ctx.SetReturnObjectByKey(bc);
            }

            var opt = (IBusinessOperationManipulate<MRegistration>)FactoryBusinessOperation.CreateBusinessOperationObject("ResetRegistration");

            MRegistration rg = new MRegistration();
            rg.Pin = "9999999999";
            rg.SerialNumber = "ABCDEFGHIJKLM";

            bool shouldThrow = !barcodeFound || !isActivated;

            if (shouldThrow)
            {
                try
                {
                    opt.Apply(rg);
                    Assert.Fail("Exception should be thrown here!!!");
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    bool foundKeyword = msg.Contains(keyword);
                    Assert.AreEqual(true, foundKeyword, "Should get [{0}] error!!!", keyword);
                }
            }
            else
            {
                //Found barcode and not yet activated
                opt.Apply(rg);

                //Status wrote back to input parameter
                Assert.AreEqual("RESET", rg.Status);
            }
        }
    }
}
