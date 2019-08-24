using System;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Magnum.Api.Utils;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Web.Utils
{
    public class ContentCacheUtilsTest
    {
        Mock<ContentCacheUtils> mockUtil;
        ContentCacheUtils util;

        ContentCacheUtils realUtil;

        [SetUp]
        public void Setup()
        {
            mockUtil = new Mock<ContentCacheUtils>() { CallBase = true };
            util = mockUtil.Object;
            realUtil = ContentCacheUtils.GetInstance();
        }

        [Test]
        public void GetInstance()
        {
            Assert.NotNull(ContentCacheUtils.GetInstance());
        }

        [Test]
        public void GetNewContentFirstTime()
        {
            var dummy = new Dictionary<string, Dictionary<string, string>>();
            dummy["TEST"] = new Dictionary<string, string>();
            mockUtil.Setup(foo => foo.LoadContents()).Returns(dummy);

            var result = util.GetContents();
            Assert.NotNull(result);
        }

        [Test]
        public void GetNewContentByRefreshTime()
        {
            var dummy = new Dictionary<string, Dictionary<string, string>>();
            dummy["TEST"] = new Dictionary<string, string>();
            util.contents = dummy;
            mockUtil.Setup(foo => foo.LoadContents()).Returns(dummy);
            mockUtil.Setup(foo => foo.IsRefreshTime()).Returns(true);

            var result = util.GetContents();
            Assert.NotNull(result);
        }

        [Test]
        public void UseCachedContent()
        {
            var dummy = new Dictionary<string, Dictionary<string, string>>();
            dummy["TEST"] = new Dictionary<string, string>();
            util.contents = dummy;

            mockUtil.Setup(foo => foo.IsRefreshTime()).Returns(false);

            var result = util.GetContents();
            Assert.NotNull(result);
        }

        [Test]
        public void NotRefreshTime()
        {
            realUtil.lastRefreshTime = DateTime.Now;
            bool result = realUtil.IsRefreshTime();
            Assert.False(result);
        }

        [Test]
        public void IsRefreshTime()
        {
            realUtil.lastRefreshTime = DateTime.Now.AddHours(-1);
            bool result = realUtil.IsRefreshTime();
            Assert.True(result);
        }

        [Test]
        public void LoadContents()
        {
            var content1 = new MContent();
            content1.Name = "001";
            content1.Type = "txt";
            content1.Value["EN"] = "one";

            var content2 = new MContent();
            content2.Name = "002";
            content2.Type = "jpg";
            content2.Value["EN"] = "two";

            var list = new List<MContent>();
            list.Add(content1);
            list.Add(content2);
            IEnumerable<MContent> dummy = (IEnumerable<MContent>)list;

            var mockOpr = new Mock<IBusinessOperationQuery<MContent>>();
            mockUtil.Setup(foo => foo.GetContentListOperation()).Returns(mockOpr.Object);
            mockOpr.Setup(foo => foo.Apply(null, null)).Returns(dummy);
            var contents = util.LoadContents();

            Assert.AreEqual("one", contents["txt/001"]["EN"]);
            Assert.AreEqual("two", contents["jpg/002"]["EN"]);
            Assert.AreEqual(2, contents.Count);
        }

        [Test]
        public void GetContentListOperation()
        {
            var opr = realUtil.GetContentListOperation();
            Assert.NotNull(opr);
        }
    }
}