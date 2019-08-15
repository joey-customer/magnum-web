using System;
using NUnit.Framework;

namespace Magnum.Consoles.Commons
{
    public class ConsoleAppBaseTest
    {
        [SetUp]
        public void Setup()
        {                     
        }

        [TestCase("firebase", 1)]
        [TestCase("dummy", 0)]
        public void GetNoSqlContextTest(string provider, int retCode)
        {
            ConsoleAppBaseMocked mocked = new ConsoleAppBaseMocked(provider);
            mocked.CreateOptionSet();
            
            try
            {
                int cd = mocked.Run();                
                Assert.AreEqual(0, cd);
            }
            catch
            {
                Assert.AreEqual("firebase", provider);
            }
        }  

        private void AddAuthenArguments(ConsoleAppBaseMocked mocked)
        {
            mocked.SetNoSqlContext(null);

            mocked.AddArgument("bucket", Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_BUCKET"));
            mocked.AddArgument("host", Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_URL"));
            mocked.AddArgument("key", Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_KEY"));
            mocked.AddArgument("user", Environment.GetEnvironmentVariable("MAGNUM_DB_USERNAME"));
            mocked.AddArgument("password", Environment.GetEnvironmentVariable("MAGNUM_DB_PASSWORD"));
        }

        [TestCase("firebase")]
        public void GetKnownStorageContextWithAuthenTest(string provider)
        {
            ConsoleAppBaseMocked mocked = new ConsoleAppBaseMocked(provider);
            AddAuthenArguments(mocked);

            var ctx = mocked.GetStorageContextWithAuthen(provider);
            Assert.AreNotEqual(null, ctx, "Returned context must not be null!!!");
        }

        [TestCase("unknown_provider")]
        public void GetUnknownStorageContextWithAuthenTest(string provider)
        {
            ConsoleAppBaseMocked mocked = new ConsoleAppBaseMocked(provider);
            AddAuthenArguments(mocked);

            var ctx = mocked.GetStorageContextWithAuthen(provider);
            Assert.AreEqual(null, ctx, "Returned context must be null!!!");
        }        

        [TestCase("firebase")]
        public void GetKnownNoSqlContextWithAuthenTest(string provider)
        {
            ConsoleAppBaseMocked mocked = new ConsoleAppBaseMocked(provider);
            AddAuthenArguments(mocked);

            var ctx1 = mocked.GetNoSqlContextWithAuthen(provider);
            var ctx2 = mocked.GetNoSqlContext();

            Assert.AreNotEqual(null, ctx1, "Returned context must not be null!!!");
            Assert.AreEqual(null, ctx2, "Returned context must be null!!!");
        }

        [TestCase("unknown_provider")]
        public void GetUnknownNoSqlContextWithAuthenTest(string provider)
        {
            ConsoleAppBaseMocked mocked = new ConsoleAppBaseMocked(provider);
            AddAuthenArguments(mocked);

            var ctx1 = mocked.GetNoSqlContextWithAuthen(provider);
            var ctx2 = mocked.GetNoSqlContext();

            Assert.AreEqual(null, ctx1, "Returned context must be null!!!");
            Assert.AreEqual(null, ctx2, "Returned context must be null!!!");
        }                          
    }    
}