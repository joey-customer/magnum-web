using System;
using NUnit.Framework;
using Magnum.Api.Commons.Business;

using Firebase.Database;

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

        [TestCase("dummy-firebase-url.com")]
        public void SetContextFromUrlTest(string url)
        {
            FactoryBusinessOperation.SetContext(url);
            var ctx = FactoryBusinessOperation.GetContext();
            Assert.IsNotNull(ctx, "Object must not be null!!!");
        }

        [TestCase("dummy-firebase-url.com")]
        public void SetContextFromObjectTest(string url)
        {
            FirebaseClient fbContext = new FirebaseClient(url);
            FactoryBusinessOperation.SetContext(fbContext);

            FirebaseClient ctx = FactoryBusinessOperation.GetContext();
            Assert.IsNotNull(ctx, "Object must not be null!!!");
        } 

        [TestCase("dummy-firebase-url.com", "CreateBarcode")]
        public void BusinessOperationGetSameContextTest(string url, string apiName)
        {
            FirebaseClient fbContext = new FirebaseClient(url);
            FactoryBusinessOperation.SetContext(fbContext);

            BusinessOperationBase opt = (BusinessOperationBase) FactoryBusinessOperation.CreateBusinessOperationObject(apiName);
            Assert.AreEqual(fbContext, opt.GetContext(), "Context must be the same object!!!");
        }              
    }    
}