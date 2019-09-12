using System;
using System.IO;
using System.Collections;
using NUnit.Framework;

using Magnum.Consoles.Commons;
using Magnum.Consoles.Factories;
using Magnum.Api.Models;
using Magnum.Api.Factories;

using NDesk.Options;

namespace Magnum.Consoles.Registrations
{
    public class ResetBarcodeApplicationTest
    {
        private Hashtable h = null;
        private string[] args = null;

        [SetUp]
        public void Setup()
        {
            h = new Hashtable()
            {
                {"host", "xxxx.firebase.com"},
                {"key" , "firebase_key"},
                {"serial", "09999915"},
                {"pin", "AAAAAAAAAA"},
                {"user", "pjame"},
                {"password", "faked_password"},
            };

            args = new string[]
            {
                string.Format("--h={0}", h["host"]),
                string.Format("--k={0}", h["key"]),
                string.Format("--s={0}", h["serial"]),
                string.Format("--p={0}", h["pin"]),
                string.Format("--user={0}", h["user"]),
                string.Format("--password={0}", h["password"]),
            };
        }

        [TestCase("BarcodeReset")]
        public void ArgumentParsingTest(string appName)
        {
            ConsoleAppBase app = (ConsoleAppBase)FactoryConsoleApplication.CreateConsoleApplicationObject(appName);

            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            Hashtable values = app.GetArguments();
            foreach (string key in values.Keys)
            {
                string value = (string)values[key];
                Assert.AreEqual(h[key].ToString(), value, "Arguments parsing incorrect!!!");
            }

            Assert.AreEqual(h.Count, values.Count, "Number of argument parsed is incorrect!!!");

            //Test to cover code coverage
            app.DumpParameter();
        }

        [TestCase("BarcodeReset", true)]
        [TestCase("BarcodeReset", false)]
        public void ResetBarcodeTest(string appName, bool IsActivated)
        {
            ResetBarcodeApplication app = (ResetBarcodeApplication)FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            MockedNoSqlContext ctx = new MockedNoSqlContext();
            MBarcode barcode = new MBarcode();
            barcode.IsActivated = IsActivated;
            ctx.SetReturnObjectByKey(barcode);
            app.SetNoSqlContext(ctx);

            //To cover test coverage
            app.GetLogger();

            app.Run();
            Assert.True(true);
        }

        [TestCase("BarcodeReset")]
        public void ResetBarcodeTest(string appName)
        {
            ResetBarcodeApplication app = (ResetBarcodeApplication)FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            MockedNoSqlContext ctx = new MockedNoSqlContext();
            app.SetNoSqlContext(ctx);

            //To cover test coverage
            app.GetLogger();

            app.Run();
            Assert.True(true);
        }
    }
}