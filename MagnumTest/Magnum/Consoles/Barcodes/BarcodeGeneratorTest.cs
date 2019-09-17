using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

using NUnit.Framework;

using Magnum.Consoles.Commons;
using Magnum.Consoles.Factories;
using Magnum.Consoles.Barcodes.ImageGenerators;
using Magnum.Consoles.Barcodes.HtmlConverters;

using Its.Onix.Erp.Models;
using Its.Onix.Core.NoSQL;

using NDesk.Options;
using Moq;

namespace Magnum.Consoles.Barcodes
{
    public class BarcodeGeneratorTest
    {
        private Hashtable h = null;
        private string[] args = null;
        private int generatedCount = 0;
        private bool imgGenerateFlag = false;

        [SetUp]
        public void Setup()
        {
            h = new Hashtable()
            {
                {"host", "xxxx.firebase.com"},
                {"key" , "firebase_key"},
                {"quantity", "15"},
                {"batch", "KH0009"},
                {"url", "https://magnum-verify.com"},
                {"outpath", "/d/temp"},
                {"user", "pjame"},
                {"profile", "ForTestingOnly"},
                {"password", "faked_password"},
            };

            args = new string[] 
            {
                string.Format("--h={0}", h["host"]), 
                string.Format("--k={0}", h["key"]), 
                string.Format("--q={0}", h["quantity"]), 
                string.Format("--b={0}", h["batch"]), 
                string.Format("--u={0}", h["url"]), 
                string.Format("--o={0}", h["outpath"]), 
                string.Format("--user={0}", h["user"]), 
                string.Format("--profile={0}", h["profile"]), 
                string.Format("--password={0}", h["password"]), 
            };                      
        }

        [TestCase("BarcodeGen", "Y")]
        public void ArgumentParsingTest(string appName, string generateFlag)
        {
            ConsoleAppBase app = (ConsoleAppBase) FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            
            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);
            if (!generateFlag.Equals(""))
            {
                app.AddArgument("generate", generateFlag);
                h.Add("generate", generateFlag);
            }

            Hashtable values = app.GetArguments();
            foreach (string key in values.Keys)
            {
                string value = (string) values[key];
                Assert.AreEqual(h[key].ToString(), value, "Arguments parsing incorrect!!!");
            }  

            //Assert.AreEqual(h.Count, values.Count, "Number of argument parsed is incorrect!!!");

            //Test to cover code coverage
            app.DumpParameter();
        }  

        private void BarcodeGenerateProgressUpdate(MBarcode bc, string dir)
        {
            generatedCount++;

            string fileName = string.Format("{0}/{1}-{2}.png", dir, bc.SerialNumber, bc.Pin);
            bool isExist = File.Exists(fileName);

            if (imgGenerateFlag)
            {
                Assert.AreEqual(true, isExist, "File not found [{0}] !!!", fileName);
            }
            else
            {
                Assert.AreEqual(false, isExist, "File should not be created [{0}] !!!", fileName);
            }
        }

        private byte[] CreateDummyBitmap()
        {            
            byte[] byteImage;
            MemoryStream ms = new MemoryStream();

            string tmpDir = Path.GetTempPath();
            string tmpPath = string.Format("{0}/{1}", tmpDir, "dummy.bmp");
            using (Bitmap bitmap = new Bitmap(100, 100))
            {
                bitmap.Save(tmpPath);
                Bitmap img = (Bitmap) Bitmap.FromFile(tmpPath);

                img.Save(ms, ImageFormat.Bmp);

                byteImage = new Byte[ms.Length];   
                byteImage = ms.ToArray();
            }
         
            return byteImage;
        }

        [TestCase("BarcodeGen", 1, true, true)]
        [TestCase("BarcodeGen", 2, false, false)]
        public void GenerateBarcodeTest(string appName, int quantity, bool callFunc, bool generateFlag)
        {
            byte[] bytes = CreateDummyBitmap();

            var mockedConverter = new Mock<IHtmlConverter>();  
            mockedConverter.Setup(p => p.FromHtmlString(It.IsAny<string>())).Returns(bytes);  

            LabelGenerator generator = new LabelGenerator();
            generator.SetHtmlConverter(mockedConverter.Object);

            generatedCount = 0;

            BarcodeGeneratorApplication app = (BarcodeGeneratorApplication) FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            app.SetLabelGnerator(generator);            
            app.SetFilePerFolder(10);
            app.SetProgressPerImage(2);
            if (callFunc)
            {
                app.SetUpdateProgressFunc(BarcodeGenerateProgressUpdate);
            }

            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            //Update the same key if already exist
            app.AddArgument("outpath", Path.GetTempPath());
            app.AddArgument("quantity", quantity.ToString());

            imgGenerateFlag = false;
            if (generateFlag)
            {
                imgGenerateFlag = true;
                app.AddArgument("generate", "Y");
            }

            INoSqlContext ctx = new Mock<INoSqlContext>().Object;
            app.SetNoSqlContext(ctx);

            app.Run();
            
            if (callFunc)
            {
                Assert.AreEqual(quantity, generatedCount, "Generated file count is wrong!!!");
            }
        }          
    }    
}