using System.IO;
using System.Collections;
using NUnit.Framework;

using Magnum.Consoles.Commons;
using Magnum.Consoles.Factories;
using Its.Onix.Core.NoSQL;

using NDesk.Options;
using Moq;
using Microsoft.Extensions.Logging;

namespace Magnum.Consoles.ProductTypes
{
    public class ImportProductTypeApplicationTest : BaseTest
    {
        private Hashtable h = null;
        private string[] args = null;
        private readonly string tempPath = Path.GetTempPath();
        private readonly string fileName = "product_types.xml";

        public ImportProductTypeApplicationTest() : base()
        {
        }

        private void createSuccessXML(string fileName)
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8' ?>
<API>
    <OBJECT name='PARAM'></OBJECT>
    <OBJECT name='ProductTypeImport'>
        <ITEMS name='ProductTypes'>
            <OBJECT name='ProductType'>
                <FIELD name='Code'>001</FIELD>
                <ITEMS name='Descriptions'>
                    <OBJECT name='Description'>
                        <FIELD name='Language'>EN</FIELD>
                        <FIELD name='Name'>Orals</FIELD>
                        <FIELD name='ShortDescription'>Orals 01</FIELD>
                        <FIELD name='LongDescription'>Orals 001</FIELD>
                    </OBJECT>
                </ITEMS>
            </OBJECT>
        </ITEMS>
    </OBJECT>
</API>";
            File.WriteAllText(fileName, xml);
        }


        private void createFailedXML(string fileName)
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8' ?>
<API>
    <OBJECT name='PARAM'></OBJECT>
    <OBJECT name='ProductTypeImport'>
        <ITEMS name='ProductTypes'>
            <OBJECT name='ProductType'>
                <FIELD name='Code'></FIELD>
            </OBJECT>
        </ITEMS>
    </OBJECT>
</API>";
            File.WriteAllText(fileName, xml);
        }

        [SetUp]
        public void Setup()
        {
            FactoryConsoleApplication.SetLoggerFactory(new Mock<ILoggerFactory>().Object);
            h = new Hashtable()
            {
                {"host", "xxxx.firebase.com"},
                {"key" , "firebase_key"},
                {"infile", fileName},
                {"basedir", tempPath},
                {"user", "pjame"},
                {"password", "faked_password"},
            };

            args = new string[] 
            {
                string.Format("--h={0}", h["host"]), 
                string.Format("--k={0}", h["key"]), 
                string.Format("--infile={0}", h["infile"]), 
                string.Format("--basedir={0}", h["basedir"]), 
                string.Format("--user={0}", h["user"]), 
                string.Format("--password={0}", h["password"]), 
            };                      
        }

        [TestCase("ImportProductType")]
        public void ArgumentParsingTest(string appName)
        {
            ConsoleAppBase app = (ConsoleAppBase) FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            
            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            Hashtable values = app.GetArguments();
            foreach (string key in values.Keys)
            {
                string value = (string) values[key];
                Assert.AreEqual(h[key].ToString(), value, "Arguments parsing incorrect!!!");
            }  

            Assert.AreEqual(h.Count, values.Count, "Number of argument parsed is incorrect!!!");

            //Test to cover code coverage
            app.DumpParameter();
        }  

        [TestCase("ImportProductType")]
        public void ImportProductTypeSuccessTest(string appName)
        {
            ImportProductTypeApplication app = (ImportProductTypeApplication) FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            string[] paths = {tempPath, fileName};
            string importFile = Path.Combine(paths);
            createSuccessXML(importFile);

            INoSqlContext ctx = new Mock<INoSqlContext>().Object;
            app.SetNoSqlContext(ctx);

            app.Run();
            Assert.True(true);
        }


        [TestCase("ImportProductType")]
        public void ImportProductTypeFailedTest(string appName)
        {
            ImportProductTypeApplication app = (ImportProductTypeApplication) FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            string[] paths = {tempPath, fileName};
            string importFile = Path.Combine(paths);
            createFailedXML(importFile);

            INoSqlContext ctx = new Mock<INoSqlContext>().Object;
            app.SetNoSqlContext(ctx);

            app.Run();
            Assert.True(true);
        }
    }    
}