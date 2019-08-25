using System;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;
using Magnum.Api.Caches;

namespace Magnum.Web.Utils
{
    public class CachePageContentsTest
    {
        Mock<CachePageContents> mockCache;
        Mock<IBusinessOperationQuery<MContent>> mockOpr;
        CachePageContents cache;
        CachePageContents realCache;


        [SetUp]
        public void Setup()
        {
            IEnumerable<MContent> dummy = getDummyContents();
            mockOpr = new Mock<IBusinessOperationQuery<MContent>>();
            mockOpr.Setup(foo => foo.Apply(null, null)).Returns(dummy);
            mockCache = new Mock<CachePageContents>() { CallBase = true };
            mockCache.Setup(foo => foo.GetContentListOperation()).Returns(mockOpr.Object);
            cache = mockCache.Object;
            realCache = new CachePageContents();
        }


        [Test]
        public void LoadContentsFirstTime()
        {
            var contents = cache.GetValues();

            Assert.AreEqual("one", ((MContent)contents["txt/001"]).Values["EN"]);
            Assert.AreEqual("two", ((MContent)contents["jpg/002"]).Values["EN"]);
            Assert.AreEqual(2, contents.Count);
        }

        [Test]
        public void GetValue()
        {
            var content = (MContent)cache.GetValue("txt/001");
            Assert.AreEqual("one", content.Values["EN"]);
        }

        [Test]
        public void LoadContentsByRefresh()
        {
            cache.SetLastRefreshDtm(DateTime.Now.AddMinutes(-6));
            cache.SetContents(new Dictionary<string, BaseModel>());
            cache.SetRefreshInterval(TimeSpan.TicksPerMinute * 5);
            var contents = cache.GetValues();
            Assert.AreEqual("one", ((MContent)contents["txt/001"]).Values["EN"]);
            Assert.AreEqual("two", ((MContent)contents["jpg/002"]).Values["EN"]);
            Assert.AreEqual(2, contents.Count);
        }

        [Test]
        public void LoadContentsNoRefresh()
        {
            cache.SetLastRefreshDtm(DateTime.Now);
            cache.SetContents(new Dictionary<string, BaseModel>());
            var contents = cache.GetValues();

            Assert.AreEqual(0, contents.Count);
        }

        [Test]
        public void GetContentListOperation()
        {
            var opr = realCache.GetContentListOperation();
            Assert.NotNull(opr);
        }

        private IEnumerable<MContent> getDummyContents()
        {
            var content1 = new MContent();
            content1.Name = "001";
            content1.Type = "txt";
            content1.Values["EN"] = "one";

            var content2 = new MContent();
            content2.Name = "002";
            content2.Type = "jpg";
            content2.Values["EN"] = "two";

            var list = new List<MContent>();
            list.Add(content1);
            list.Add(content2);
            IEnumerable<MContent> dummy = (IEnumerable<MContent>)list;
            return dummy;
        }
    }
}