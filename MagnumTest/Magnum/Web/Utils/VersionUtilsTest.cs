using NUnit.Framework;

namespace Magnum.Web.Utils
{
    public class VersionUtilsTest
    {
        [Test]
        public void GetVersion()
        {
            string result = VersionUtils.GetVersion();
            Assert.AreEqual("1.0.0.0", result);
        }
    }
}