using System;
using NUnit.Framework;
using Moq;
using Magnum.Api.Caches;
using Microsoft.Extensions.Logging;

namespace Magnum.Web.Utils
{
    public class CacheBaseTest
    {
        Mock<CachePageContents> mockCache;
        CachePageContents cache;

        [SetUp]
        public void Setup()
        {
            mockCache = new Mock<CachePageContents>() { CallBase = true };
            cache = mockCache.Object;
        }

        [Test]
        public void AssignLoggerTest()
        {
            var logger = new Mock<ILogger>().Object;
            cache.SetLogger(logger);
            var outLogger = cache.GetLogger();
            Assert.True(logger == outLogger);
        }

        [Test]
        public void GetLastRefreshDtm()
        {
            DateTime currentDtm = DateTime.Now;
            cache.SetLastRefreshDtm(currentDtm);
            DateTime outDtm = cache.GetLastRefreshDtm();
            Assert.True(currentDtm == outDtm);
        }

    }
}