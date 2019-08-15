using System;
using NUnit.Framework;

using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Products
{
	public class GetProductListTest
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

            var opt = (IBusinessOperationQuery<MProduct>) FactoryBusinessOperation.CreateBusinessOperationObject("GetProductList");
            
            MProduct pd = new MProduct();
            pd.Code = code;
            
            try
            {
                opt.Apply(pd, null);                
            }
            catch (Exception)
            {
                Assert.Fail("Exception should not be thrown here!!!");
            }
        }        
    }
}
