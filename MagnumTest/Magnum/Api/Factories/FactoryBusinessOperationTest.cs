using System;
using NUnit.Framework;
using Magnum.Api.Commons.Business;

using Magnum.Api.NoSql;

namespace Magnum.Api.Factories
{
    public class FactoryBusinessOperationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("UnknowAPI")]
        public void UnknownApiNameTest(string apiName)
        {
            try
            {
                IBusinessOperation opt = FactoryBusinessOperation.CreateBusinessOperationObject(apiName);
                Assert.True(false, "Exception shoud be throw for unknow API!!!");
            }
            catch (Exception e)
            {
                Assert.True(true, e.Message);
            }
        } 

        [TestCase("CreateBarcode")]
        public void KnownApiNameTest(string apiName)
        {
            IBusinessOperation opt = FactoryBusinessOperation.CreateBusinessOperationObject(apiName);
            Assert.IsNotNull(opt, "Object must not be null!!!");
        } 

        [TestCase("dummy-firebase-url.com", "faked_key")]
        public void SetContextFromObjectTest(string url, string key)
        {
            INoSqlContext fbContext = new MockedNoSqlContext();
            fbContext.Authenticate(url, key, "", "");
            FactoryBusinessOperation.SetNoSqlContext(fbContext);

            //Must not die if pass null
            FactoryBusinessOperation.SetSmtpContext(null);

            INoSqlContext ctx = FactoryBusinessOperation.GetNoSqlContext();
            Assert.IsNotNull(ctx, "Object must not be null!!!");
        } 

        [TestCase("dummy-firebase-url.com", "faked_key", "CreateBarcode")]
        public void BusinessOperationGetSameContextTest(string url, string key, string apiName)
        {
            INoSqlContext fbContext = new MockedNoSqlContext();
            fbContext.Authenticate(url, key, "", "");
            FactoryBusinessOperation.SetNoSqlContext(fbContext);

            BusinessOperationBase opt = (BusinessOperationBase) FactoryBusinessOperation.CreateBusinessOperationObject(apiName);
            Assert.AreEqual(fbContext, opt.GetNoSqlContext(), "Context must be the same object!!!");
        }              
    }    
}