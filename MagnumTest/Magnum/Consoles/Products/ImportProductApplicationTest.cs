using System.IO;
using System.Collections;
using NUnit.Framework;

using Magnum.Consoles.Commons;
using Magnum.Consoles.Factories;
using Its.Onix.Core.NoSQL;
using Its.Onix.Core.Storages;

using Moq;
using NDesk.Options;

namespace Magnum.Consoles.Products
{
    public class ImportProductApplicationTest : BaseTest
    {
        private Hashtable h = null;
        private string[] args = null;
        private readonly string tempPath = Path.GetTempPath();
        private readonly string fileName = "products.xml";

        public ImportProductApplicationTest() : base()
        {
        }

        private void createSuccessXML(string fileName)
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8' ?>
<API>
    <OBJECT name='PARAM'></OBJECT>

    <OBJECT name='ProductImport'>
        <ITEMS name='Products'>

            <OBJECT name='Product'>
                <FIELD name='Code'>Oral-AAA-001</FIELD>
                <FIELD name='Rating'>4</FIELD>
                <FIELD name='ProductType'>001</FIELD>
                <FIELD name='Price'>200</FIELD>
                <FIELD name='Image1LocalPath'>Orals/pro-img1.png</FIELD>
                
                <ITEMS name='Descriptions'>
                    <OBJECT name='Description'>
                        <FIELD name='Language'>EN</FIELD>
                        <FIELD name='Name'>PRODUCT TITLE HERE #1</FIELD>
                        <FIELD name='ShortDescription'>Product Long Name, 300 mg/ml – 10 x 1ml Amps</FIELD>
                        <FIELD name='LongDescription1'>Qnteate Supple Chan Though Marke Poston Bestng Practcese Marke Supple Chan Though Marke Poston Bestng Practces Chain Throuh Practces eractve Fashion Fashion Economically And Sound</FIELD>
                        <FIELD name='LongDescription2'>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo</FIELD>
                    </OBJECT>
                </ITEMS>

                <ITEMS name='CompositionGroups'>
                    <OBJECT name='CompositionGroup'>
                        <FIELD name='PerUnit'>1 Tab</FIELD>
                        <ITEMS name='Compositions'>
                            <OBJECT name='Composition'>
                                <FIELD name='Code'>1</FIELD>
                                <FIELD name='Quantity'>3</FIELD>
                                <FIELD name='Unit'>ml</FIELD>
                                <ITEMS name='Descriptions'>
                                    <OBJECT name='Description'>
                                        <FIELD name='Language'>EN</FIELD>
                                        <FIELD name='Name'>H2O</FIELD>
                                    </OBJECT>
                                </ITEMS>                        
                            </OBJECT>               
                        </ITEMS>
                    </OBJECT>
                </ITEMS>
                
                <ITEMS name='Performances'>
                    <OBJECT name='Performance'>
                        <FIELD name='Code'>1</FIELD>
                        <FIELD name='Quantity'>3</FIELD>
                        <FIELD name='Unit'>Star</FIELD>

                        <ITEMS name='Descriptions'>
                            <OBJECT name='Description'>
                                <FIELD name='Language'>EN</FIELD>
                                <FIELD name='Name'>Strength</FIELD>
                            </OBJECT>
                        </ITEMS>                        
                    </OBJECT>

                    <OBJECT name='Performance'>
                        <FIELD name='Code'>2</FIELD>
                        <FIELD name='Quantity'>4</FIELD>
                        <FIELD name='Unit'>Star</FIELD>

                        <ITEMS name='Descriptions'>
                            <OBJECT name='Description'>
                                <FIELD name='Language'>EN</FIELD>
                                <FIELD name='Name'>Muscle</FIELD>
                            </OBJECT>
                        </ITEMS>                        
                    </OBJECT>

                    <OBJECT name='Performance'>
                        <FIELD name='Code'>3</FIELD>
                        <FIELD name='Quantity'>3</FIELD>
                        <FIELD name='Unit'>Star</FIELD>

                        <ITEMS name='Descriptions'>
                            <OBJECT name='Description'>
                                <FIELD name='Language'>EN</FIELD>
                                <FIELD name='Name'>Fat/Water Loss</FIELD>
                            </OBJECT>
                        </ITEMS>                        
                    </OBJECT>

                    <OBJECT name='Performance'>
                        <FIELD name='Code'>4</FIELD>
                        <FIELD name='Quantity'>2</FIELD>
                        <FIELD name='Unit'>Star</FIELD>

                        <ITEMS name='Descriptions'>
                            <OBJECT name='Description'>
                                <FIELD name='Language'>EN</FIELD>
                                <FIELD name='Name'>Side Effect</FIELD>
                            </OBJECT>
                        </ITEMS>                        
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
    <OBJECT name='ProductImport'>
        <ITEMS name='Products'>
            <OBJECT name='Product'>
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

        [TestCase("ImportProduct")]
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

        [TestCase("ImportProduct")]
        public void ImportProductTypeSuccessTest(string appName)
        {
            ImportProductApplication app = (ImportProductApplication) FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            string[] paths = {tempPath, fileName};
            string importFile = Path.Combine(paths);
            createSuccessXML(importFile);

            INoSqlContext ctx = new Mock<INoSqlContext>().Object;
            app.SetNoSqlContext(ctx);

            IStorageContext storageCtx = new Mock<IStorageContext>().Object;
            app.SetStorageContext(storageCtx);

            app.Run();
            Assert.True(true);
        }


        [TestCase("ImportProduct")]
        public void ImportProductTypeFailedTest(string appName)
        {
            ImportProductApplication app = (ImportProductApplication) FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
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