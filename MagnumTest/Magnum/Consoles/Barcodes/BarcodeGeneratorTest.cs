using System;
using System.IO;
using System.Collections;
using NUnit.Framework;

using Magnum.Consoles.Commons;
using Magnum.Consoles.Factories;
using Magnum.Api.Models;

using NDesk.Options;

namespace Magnum.Consoles.Barcodes
{
    public class BarcodeGeneratorTest
    {
        private Hashtable h = null;
        private string[] args = null;
        private int generatedCount = 0;

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
                {"product", "ALPHA-099"},
                {"outpath", "/d/temp"},
            };

            args = new string[] 
            {
                string.Format("--h={0}", h["host"]), 
                string.Format("--k={0}", h["key"]), 
                string.Format("--q={0}", h["quantity"]), 
                string.Format("--b={0}", h["batch"]), 
                string.Format("--u={0}", h["url"]), 
                string.Format("--p={0}", h["product"]), 
                string.Format("--o={0}", h["outpath"]), 
            };                      
        }

        [TestCase("BarcodeGen")]
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

        private void BarcodeGenerateProgressUpdate(MBarcode bc, string dir)
        {
            generatedCount++;

            string fileName = string.Format("{0}/{1}-{2}.png", dir, bc.SerialNumber, bc.Pin);
            bool isExist = File.Exists(fileName);

            Assert.AreEqual(true, isExist, "File not found [{0}] !!!", fileName);
        }

        [TestCase("BarcodeGen", 20, true)]
        [TestCase("BarcodeGen", 20, false)]
        public void GenerateBarcodeTest(string appName, int quantity, bool callFunc)
        {
            generatedCount = 0;

            BarcodeGenerator app = (BarcodeGenerator) FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            app.SetFilePerFoler(10);
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

            app.Run();
            
            if (callFunc)
            {
                Assert.AreEqual(quantity, generatedCount, "Gerated file count is wrong!!!");
            }
        }          
    }    
}