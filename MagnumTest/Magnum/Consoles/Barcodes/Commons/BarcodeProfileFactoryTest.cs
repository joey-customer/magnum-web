using System;
using System.Collections;

using NUnit.Framework;

namespace Magnum.Consoles.Barcodes.Commons
{
    public class BarcodeProfileFactoryTest : BaseTest
    {
        public BarcodeProfileFactoryTest() : base()
        {
        }

        [SetUp]
        public void Setup()
        {
        }

        [TestCase("UnknownProfileName")]
        public void UnknownApplicationNameTest(string profName)
        {
            try
            {
                IBarcodeProfile opt = BarcodeProfileFactory.CreateBarcodeProfileObject(profName);
                Assert.True(false, "Exception shoud be throw for unknow application name!!!");
            }
            catch (Exception e)
            {
                Assert.True(true, e.Message);
            }
        } 

        [TestCase("ForTestingOnly")]
        public void KnownApplicationNameTest(string profName)
        {
            IBarcodeProfile opt = BarcodeProfileFactory.CreateBarcodeProfileObject(profName);
            Assert.IsNotNull(opt, "Object must not be null!!!");
        } 

        [TestCase]
        public void KnowProfileShouldBeInstantiableTest()
        {
            Hashtable knownLists = BarcodeProfileFactory.GetKnownClassList();
            foreach (string profileName in knownLists.Keys)
            {
                IBarcodeProfile opt = BarcodeProfileFactory.CreateBarcodeProfileObject(profileName);
                opt.Setup();
                Assert.IsNotNull(opt, "Object must not be null!!! [{0}]", profileName);
            }            
        }                          
    }    
}