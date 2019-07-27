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

        [TestCase]
        public void CreateRegistrationWithCodeEmptyTest()
        {
            var opt = (IBusinessOperationManipulate<MRegistration>) FactoryBusinessOperation.CreateBusinessOperationObject("CreateRegistration");
            MRegistration bc = new MRegistration();
            opt.Apply(bc);
        } 
    }
}
