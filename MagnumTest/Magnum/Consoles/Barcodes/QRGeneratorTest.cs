using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

using NUnit.Framework;

using Magnum.Consoles.Factories;
using Magnum.Consoles.Barcodes.ImageGenerators;
using Magnum.Consoles.Barcodes.HtmlConverters;

using NDesk.Options;
using Moq;

namespace Magnum.Consoles.Barcodes
{
    public class QRGeneratorTest : BaseTest
    {
        private Hashtable h = null;
        private string[] args = null;

        public QRGeneratorTest() : base()
        {
        }

        [SetUp]
        public void Setup()
        {
            h = new Hashtable()
            {
                {"outpath", "/d/temp"},
                {"profile", "ForQrTestingOnly"},
            };

            args = new string[] 
            {
                string.Format("--o={0}", h["outpath"]), 
                string.Format("--profile={0}", h["profile"]), 
            };                      
        }

        [TestCase("QrGen")]
        public void ArgumentParsingTest(string appName)
        {
            QRGeneratorApplication app = (QRGeneratorApplication) FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            
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

        private byte[] CreateDummyBitmap()
        {            
            byte[] byteImage;
            MemoryStream ms = new MemoryStream();

            string tmpDir = Path.GetTempPath();
            string[] paths = {tmpDir, "dummy.bmp"};
            string tmpPath = Path.Combine(paths);

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

        [TestCase("QrGen")]
        public void GenerateQrTest(string appName)
        {
            byte[] bytes = CreateDummyBitmap();

            var mockedConverter = new Mock<IHtmlConverter>();  
            mockedConverter.Setup(p => p.FromHtmlString(It.IsAny<string>())).Returns(bytes);  

            QRGenerator generator = new QRGenerator();
            generator.SetHtmlConverter(mockedConverter.Object);

            QRGeneratorApplication app = (QRGeneratorApplication) FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            app.SetQrGnerator(generator);

            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            //Update the same key if already exist
            app.AddArgument("outpath", Path.GetTempPath());
            app.Run();        
        }          
    }    
}