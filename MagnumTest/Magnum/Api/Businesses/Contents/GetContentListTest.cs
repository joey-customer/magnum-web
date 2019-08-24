using System;
using NUnit.Framework;

using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Contents
{
	public class GetContentListTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("")]
        public void GenericGetProductListTest(string code)
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            var opt = (IBusinessOperationQuery<MContent>) FactoryBusinessOperation.CreateBusinessOperationObject("GetContentList");
            
            try
            {
                opt.Apply(null, null);                
            }
            catch (Exception)
            {
                Assert.Fail("Exception should not be thrown here!!!");
            }
        }        
    }
}
