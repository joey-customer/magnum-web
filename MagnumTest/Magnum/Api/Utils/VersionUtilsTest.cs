using System;
using NUnit.Framework;

namespace Magnum.Api.Utils
{
    public class VersionUtilsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetVersionNumberTest()
        {
            string version = VersionUtils.GetVersion();
            Assert.AreEqual("1.0.0.0", version, "Version incorrect !!!");         
        }
    }
}