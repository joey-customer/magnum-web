using System;
using NUnit.Framework;

namespace Magnum.Web.Utils
{
    public class StringUtilsTest
    {
        [TestCase("<body>test</body>", "test")]
        [TestCase("<script>test</script>", "test")]
        [TestCase("<scripttest</script>", "")]
        [TestCase("3 < 2", "3 < 2")]
        [TestCase("","")]
        [TestCase(null,"")]
        public void StripTagsRegexTest(String data, String expected)
        {
            string result = StringUtils.StripTagsRegex(data);
            Assert.AreEqual(expected, result);
        }
    }
}