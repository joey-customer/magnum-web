using System;
using NUnit.Framework;
using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Registrations
{
	public class CreateRegistrationTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("192.168.0.1", "9999999999", "ABCDEFGHIJKLM")]
        public void CreateRegistrationWithCodeNotEmptyTest(string ip, string serial, string pin)
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetContext(ctx);

            var opt = (IBusinessOperationManipulate<MRegistration>) FactoryBusinessOperation.CreateBusinessOperationObject("CreateRegistration");
            
            MRegistration rg = new MRegistration();
            rg.Pin = pin;
            rg.SerialNumber = serial;
            rg.IP = ip;
            
            opt.Apply(rg);
        } 

        [TestCase("192.168.0.1", "9999999999", "")]
        [TestCase("192.168.0.1", "", "")]
        [TestCase("", "", "")]
        [TestCase("", "9999999999", "")]
        [TestCase("", "9999999999", "AAAAAAAAAA")]
        public void CreateRegistrationWithEmptyTest(string ip, string serial, string pin)
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetContext(ctx);

            var opt = (IBusinessOperationManipulate<MRegistration>) FactoryBusinessOperation.CreateBusinessOperationObject("CreateRegistration");
            
            MRegistration rg = new MRegistration();
            rg.Pin = pin;
            rg.SerialNumber = serial;
            rg.IP = ip;
            
            try
            {
                opt.Apply(rg);
                Assert.Fail("Exception should be thrown");
            }
            catch (Exception)
            {
                //Do nothing
            }
        }         
    }
}
