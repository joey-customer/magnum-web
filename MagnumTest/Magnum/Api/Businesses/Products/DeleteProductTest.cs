using System;
using NUnit.Framework;

using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Products
{
	public class DeleteProductTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("")]
        public void DeleteProductWithEmptyTest(string code)
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            var opt = (IBusinessOperationManipulate<MProduct>) FactoryBusinessOperation.CreateBusinessOperationObject("DeleteProduct");
            
            MProduct pd = new MProduct();
            pd.Code = code;
            
            try
            {
                opt.Apply(pd);
                Assert.Fail("Exception should be thrown");
            }
            catch (Exception)
            {
                //Do nothing
            }
        } 

        [TestCase]
        public void DeleteProductAnyCaseTest()
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();

            FactoryBusinessOperation.SetNoSqlContext(ctx);
            var opt = (IBusinessOperationManipulate<MProduct>) FactoryBusinessOperation.CreateBusinessOperationObject("DeleteProduct");
            
            MProduct pd = new MProduct();
            pd.Code = "MockedCode";
            pd.Key = "FAKED_NOTFOUNDKEY";

            var result = opt.Apply(pd);
            Assert.AreEqual(1, result, "Row affected should be returned !!!");
        }           
    }
}
