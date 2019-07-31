using System;
using System.IO;
using System.Collections;
using NUnit.Framework;

using Magnum.Consoles.Factories;
using Magnum.Api.Factories;

using NDesk.Options;

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
    }    
}