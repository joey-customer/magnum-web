using System;
using NUnit.Framework;

namespace Magnum.Consoles
{
    public class BarcodeGeneratorTest
    {
        [SetUp]
        public void Setup()
        {                     
        }

        [TestCase("DummyApp")]
        [TestCase("")]
        public void RunMainTest(string appName)
        {
            string[] args = new string[] {};
            if (!string.IsNullOrEmpty(appName))
            {
                args = new string[] { appName };
            }

            Program.Main(args);

            //Make sure no exception earlier
            Assert.True(true);
        }        
    }    
}