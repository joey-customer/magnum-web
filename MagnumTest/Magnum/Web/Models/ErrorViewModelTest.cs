using System;
using NUnit.Framework;

namespace Magnum.Web.Models
{
    public class ErroriewMovelTest
    {
        [TestCase("12345", true)]
        [TestCase("", false)]
        public void StripTagsRegexTest(String requestId, bool expected)
        {
            ErrorViewModel model = new ErrorViewModel();
            model.RequestId = requestId;
            Assert.AreEqual(expected, model.ShowRequestId);
        }
    }
}