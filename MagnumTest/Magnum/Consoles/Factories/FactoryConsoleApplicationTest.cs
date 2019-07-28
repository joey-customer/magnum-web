using System;
using NUnit.Framework;

using Magnum.Consoles.Commons;

namespace Magnum.Consoles.Factories
{
    public class FactoryConsoleApplicationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("UnknownAppName")]
        public void UnknownApplicationNameTest(string appName)
        {
            try
            {
                IConsoleApp opt = FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
                Assert.True(false, "Exception shoud be throw for unknow application name!!!");
            }
            catch (Exception e)
            {
                Assert.True(true, e.Message);
            }
        } 

        [TestCase("BarcodeGen")]
        public void KnownApplicationNameTest(string appName)
        {
            IConsoleApp opt = FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            Assert.IsNotNull(opt, "Object must not be null!!!");
        }          
    }    
}