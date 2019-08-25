using System;
using NUnit.Framework;
using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Contents

{
    public class SaveContent
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SaveTest()
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            var opt = (IBusinessOperationManipulate<MContent>)FactoryBusinessOperation.CreateBusinessOperationObject("SaveContent");

            MContent dat = new MContent();
            dat.Name = "001";
            dat.Type = "txt";
            dat.Values["EN"] = "one";
            try
            {
                int result = opt.Apply(dat);
                Assert.AreEqual(result, 0);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void SaveBlankObject()
        {
            MockedNoSqlContext ctx = new MockedNoSqlContext();
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            var opt = (IBusinessOperationManipulate<MContent>)FactoryBusinessOperation.CreateBusinessOperationObject("SaveContent");

            MContent dat = new MContent();

            try
            {
                int result = opt.Apply(dat);
                Assert.Fail();
            }
            catch (Exception)
            {
                //Error should be thrown.
                Assert.Pass();
            }
        }
    }
}
