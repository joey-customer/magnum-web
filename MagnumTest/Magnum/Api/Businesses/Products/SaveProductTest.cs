using System;
using NUnit.Framework;

using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Products
{
	public class SaveProductTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("")]
        public void SaveProductWithEmptyTest(string code)
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            MockedStorageContext storageCtx = new MockedStorageContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);
            FactoryBusinessOperation.SetStorageContext(storageCtx);

            var opt = (IBusinessOperationGetInfo<MProduct>) FactoryBusinessOperation.CreateBusinessOperationObject("SaveProduct");
            
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

        [TestCase("CODEFOUND001")]
        [TestCase("CODEFOUND002")]
        public void SaveProductByUpdatingTheExistingOneTest(string code)
        {
            MProduct prdReturned = new MProduct();
            prdReturned.Code = code;

            MockedStorageContext storageCtx = new MockedStorageContext();
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            ctx.SetReturnObjectByKey(prdReturned);

            FactoryBusinessOperation.SetNoSqlContext(ctx);
            FactoryBusinessOperation.SetStorageContext(storageCtx);
            var opt = (IBusinessOperationGetInfo<MProduct>) FactoryBusinessOperation.CreateBusinessOperationObject("SaveProduct");
            
            MProduct pd = new MProduct();
            pd.Code = "MockedCode";
            pd.Key = "FAKED_NOTFOUNDKEY";

            var result = opt.Apply(pd);
            Assert.AreEqual(code, result.Code, "Return product code should be [{0}]!!!", code);            
        } 

        [TestCase]
        public void SaveProductByAddingTheNewOneTest()
        {
            MProduct prdReturned = null;

            MockedStorageContext storageCtx = new MockedStorageContext();
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            ctx.SetReturnObjectByKey(prdReturned);            

            FactoryBusinessOperation.SetNoSqlContext(ctx);
            FactoryBusinessOperation.SetStorageContext(storageCtx);

            var opt = (IBusinessOperationGetInfo<MProduct>) FactoryBusinessOperation.CreateBusinessOperationObject("SaveProduct");
            
            MProduct pd = new MProduct();
            pd.Code = "MockedCode";
            pd.Key = "FAKED_NOTFOUNDKEY";

            var result = opt.Apply(pd);
            Assert.AreEqual(null, result, "Expected null to return !!!");            
        }         
    }
}
