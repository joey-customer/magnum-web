using System;
using NUnit.Framework;

using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.ProductTypes
{
	public class GetProductTypeListTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("")]
        public void GenericGetProductTypeListTest(string code)
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            var opt = (IBusinessOperationQuery<MProductType>) FactoryBusinessOperation.CreateBusinessOperationObject("GetProductTypeList");
            
            MProductType pd = new MProductType();
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
