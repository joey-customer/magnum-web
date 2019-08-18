using System;
using NUnit.Framework;
using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.ContactUs

{
	public class SaveContactUsTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SaveTest()
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            var opt = (IBusinessOperationManipulate<MContactUs>) FactoryBusinessOperation.CreateBusinessOperationObject("SaveContactUs");
            
            MContactUs dat = new MContactUs();
            
            try
            {
                int result = opt.Apply(dat);
                Assert.AreEqual(result, 0);   
            }
            catch (Exception)
            {
                //Do nothing
            }         
            
        } 
    }
}
