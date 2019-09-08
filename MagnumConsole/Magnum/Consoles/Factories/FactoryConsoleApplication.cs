using System;
using System.Collections;
using System.Reflection;

using Microsoft.Extensions.Logging;

using Magnum.Consoles.Commons;

namespace Magnum.Consoles.Factories
{   
    public static class FactoryConsoleApplication
    {
        private static ILoggerFactory loggerFactory = null;

        private static Hashtable classMaps = new Hashtable();

        private static void addClassConfig(string apiName, string fqdn)
        {
            classMaps.Add(apiName, fqdn);
        }

        static FactoryConsoleApplication()
        {
            initClassMap();
        }

        private static void initClassMap()
        {            
            addClassConfig("BarcodeGen", "Magnum.Consoles.Barcodes.BarcodeGeneratorApplication"); 
            addClassConfig("QrGen", "Magnum.Consoles.Barcodes.QRGeneratorApplication");             
            addClassConfig("BarcodeReg", "Magnum.Consoles.Registrations.RegisterBarcodeApplication");
            addClassConfig("BarcodeReset", "Magnum.Consoles.Registrations.ResetBarcodeApplication"); 
            addClassConfig("ImportProductType", "Magnum.Consoles.ProductTypes.ImportProductTypeApplication"); 
            addClassConfig("ImportProduct", "Magnum.Consoles.Products.ImportProductApplication");
            addClassConfig("ImportContent", "Magnum.Consoles.Contents.ImportContentApplication"); 
            
            addClassConfig("DummyApp", "Magnum.Consoles.Miscs.DummyApplication");       
        }  

        public static IConsoleApp CreateConsoleApplicationObject(string name)
        {        
            string className = (string)classMaps[name];
            if (className == null)
            {
                throw new ArgumentNullException(String.Format("Application not found [{0}]", name));
            }

            Assembly asm = Assembly.GetExecutingAssembly();
            IConsoleApp obj = (IConsoleApp)asm.CreateInstance(className);

            if (loggerFactory != null)
            {
                Type t = obj.GetType();
                ILogger logger = loggerFactory.CreateLogger(t);
                obj.SetLogger(logger);
            }
            
            return(obj);
        }

        public static void SetLoggerFactory(ILoggerFactory logFact)
        {
            loggerFactory = logFact;
        }

        public static ILoggerFactory GetLoggerFactory()
        {
            return loggerFactory;
        }  
    }
 
}