using System;
using NUnit.Framework;

using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Products
{
	public class GetProductInfoTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("", "", "")]
        [TestCase("", "CODE0001", "")]
        [TestCase("KEY0001", "", "")]
        [TestCase("KEY0001", "CODE0001", "")]
        [TestCase("KEY0001", "", "EN")]
        [TestCase("", "", "EN")]
        public void GetProductWithEmptyTest(string key, string code, string language)
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            var opt = (IBusinessOperationGetInfo<MProduct>) FactoryBusinessOperation.CreateBusinessOperationObject("GetProductInfo");
            
            MProduct pd = new MProduct();
            pd.Key = key;
            pd.Language = language;
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
        public void GetProductNotFoundTest()
        {
            MProduct prdReturned = null;
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            ctx.SetReturnObjectByKey(prdReturned);

            FactoryBusinessOperation.SetNoSqlContext(ctx);
            var opt = (IBusinessOperationGetInfo<MProduct>) FactoryBusinessOperation.CreateBusinessOperationObject("GetProductInfo");
            
            MProduct pd = new MProduct();
            pd.Language = "EN";
            pd.Code = "MockedCode";
            pd.Key = "FAKED_NOTFOUNDKEY";

            var result = opt.Apply(pd);
            Assert.AreEqual(null, result, "Product not found should return null!!!");
        } 

        [TestCase("CODEFOUND001")]
        [TestCase("CODEFOUND002")]
        public void GetProductFoundTest(string code)
        {
            MProduct prdReturned = new MProduct();
            prdReturned.Code = code;

            MockedNoSqlContext ctx = new MockedNoSqlContext();
            ctx.SetReturnObjectByKey(prdReturned);

            FactoryBusinessOperation.SetNoSqlContext(ctx);
            var opt = (IBusinessOperationGetInfo<MProduct>) FactoryBusinessOperation.CreateBusinessOperationObject("GetProductInfo");
            
            MProduct pd = new MProduct();
            pd.Language = "EN";
            pd.Code = "MockedCode";
            pd.Key = "FAKED_NOTFOUNDKEY";

            var result = opt.Apply(pd);
            Assert.AreEqual(code, result.Code, "Return product code should be [{0}]!!!", code);
        }               
    }
}
