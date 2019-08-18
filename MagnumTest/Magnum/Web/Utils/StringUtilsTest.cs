using System;
using NUnit.Framework;
using Moq;
using Magnum.Api.Businesses.Registrations;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Magnum.Web.Utils
{
    public class StringUtilsTest
    {
        [TestCase("<body>test</body>", "test")]
        [TestCase("<script>test</script>", "test")]
        [TestCase("<scripttest</script>", "")]
        [TestCase("3 < 2", "3 < 2")]
        public void StripTagsRegexTest(String data, String expected)
        {
            string result = StringUtils.StripTagsRegex(data);
            Assert.AreEqual(result, expected);
        }
    }
}