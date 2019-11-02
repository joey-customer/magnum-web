using System;
using System.IO;
using System.Collections;

using NUnit.Framework;

using Magnum.Consoles.Factories;

using Its.Onix.Erp.Models;
using Its.Onix.Core.NoSQL;
using Its.Onix.Core.Applications;

using NDesk.Options;
using Moq;
using Microsoft.Extensions.Logging;

namespace Magnum.Consoles.Barcodes
{
    public class BarcodeMigrateTest : BaseTest
    {
        private Hashtable h = null;
        private string[] args = null;
        private int generatedCount = 0;
        private bool imgGenerateFlag = false;

        public BarcodeMigrateTest() : base()
        {
        }

        [SetUp]
        public void Setup()
        {
            string infile = Path.GetTempPath() + "import_test.txt";

            h = new Hashtable()
            {
                {"infile", infile},
                {"url", "https://magnum-verify.com"},
                {"user", "pjame"},
                {"profile", "ForTestingOnly"},
                {"password", "faked_password"},
            };

            args = new string[]
            {
                string.Format("--u={0}", h["url"]),
                string.Format("--if={0}", h["infile"]),
                string.Format("--user={0}", h["user"]),
                string.Format("--profile={0}", h["profile"]),
                string.Format("--password={0}", h["password"]),
            };
        }

        [Test]
        public void ArgumentParsingTest()
        {
            ConsoleAppBase app = (ConsoleAppBase)FactoryConsoleApplication.CreateConsoleApplicationObject("BarcodeMigrate");


            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            Hashtable values = app.GetArguments();
            foreach (string key in values.Keys)
            {
                string value = (string)values[key];
                Assert.AreEqual(h[key].ToString(), value, "Arguments parsing incorrect!!!");
            }

            //Assert.AreEqual(h.Count, values.Count, "Number of argument parsed is incorrect!!!");

            //Test to cover code coverage
            app.DumpParameter();
        }

        [Test]
        public void ExecuteTest()
        {
            ConsoleAppBase app = (ConsoleAppBase)FactoryConsoleApplication.CreateConsoleApplicationObject("BarcodeMigrate");

            MBarcode existingData = new MBarcode();

            Mock<INoSqlContext> mockCtx = new Mock<INoSqlContext>();
            mockCtx.Setup(foo => foo.GetObjectByKey<MBarcode>("barcodes/8/0/5/2/1/5/8059699639/2154237147")).Returns(existingData);
            INoSqlContext ctx = mockCtx.Object;
            app.SetNoSqlContext(ctx);

            ILogger logger = new Mock<ILogger>().Object;
            app.SetLogger(logger);

            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            createInputFile((String)h["infile"]);

            int result = app.Run();

            Assert.AreEqual(0, result);
        }

        private void createInputFile(string fileName)
        {
            string content = @"Serial, PIN, QR (Url), Barcode, Web Site
7058312468,8284783723,https://magnum-pharmacy-verify.com/verification/MgnmAnastrol_prod001_20190817012440/000000/7058312468/8284783723,ANASTROL,www.magnum-pharmacy.com

8059699639,2154237147,https://magnum-pharmacy-verify.com/verification/MgnmAnastrol_prod001_20190817012440/000000/8059699639/2154237147,ANASTROL,www.magnum-pharmacy.com
2329843730,4326989925,https://magnum-pharmacy-verify.com/verification/MgnmAnastrol_prod001_20190817012440/000000/2329843730/4326989925,ANASTROL,www.magnum-pharmacy.com
";
            File.WriteAllText(fileName, content);
        }
    }
}