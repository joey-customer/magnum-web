using System;
using System.Collections;
using System.Reflection;
using Magnum.Api.NoSql;
using Magnum.Api.Storages;
using Magnum.Api.Smtp;

using Microsoft.Extensions.Logging;

using Magnum.Api.Commons.Business;

namespace Magnum.Api.Factories
{   
    public static class FactoryBusinessOperation
    {
        private static ILoggerFactory loggerFactory = null;

        private static Hashtable classMaps = new Hashtable();
        private static INoSqlContext noSqlContext = null;
        private static IStorageContext storageContext = null;
        private static ISmtpContext smtpContext = null;

        private static void addClassConfig(string apiName, string fqdn)
        {
            classMaps.Add(apiName, fqdn);
        }

        static FactoryBusinessOperation()
        {
            initClassMap();
        }

        private static void initClassMap()
        {
            addClassConfig("CreateBarcode", "Magnum.Api.Businesses.Barcodes.CreateBarcode");
            addClassConfig("CreateRegistration", "Magnum.Api.Businesses.Registrations.CreateRegistration");                        
            
            addClassConfig("SaveProduct", "Magnum.Api.Businesses.Products.SaveProduct");
            addClassConfig("DeleteProduct", "Magnum.Api.Businesses.Products.DeleteProduct");
            addClassConfig("GetProductInfo", "Magnum.Api.Businesses.Products.GetProductInfo");
            addClassConfig("GetProductList", "Magnum.Api.Businesses.Products.GetProductList");   

            addClassConfig("GetProductTypeList", "Magnum.Api.Businesses.ProductTypes.GetProductTypeList"); 
            addClassConfig("GetProductTypeInfo", "Magnum.Api.Businesses.ProductTypes.GetProductTypeInfo");
            addClassConfig("SaveProductType", "Magnum.Api.Businesses.ProductTypes.SaveProductType");

            addClassConfig("SaveContactUs", "Magnum.Api.Businesses.ContactUs.SaveContactUs");

            addClassConfig("GetContentList", "Magnum.Api.Businesses.Contents.GetContentList");
            addClassConfig("SaveContent", "Magnum.Api.Businesses.Contents.SaveContent");
        }

        public static void SetNoSqlContext(INoSqlContext ctx)
        {
            noSqlContext = ctx;
        }        

        public static INoSqlContext GetNoSqlContext()
        {
            return noSqlContext;
        }    

        public static void SetStorageContext(IStorageContext ctx)
        {
            storageContext = ctx;
        }        

        public static void SetSmtpContext(ISmtpContext ctx)
        {
            smtpContext = ctx;
        }  

        public static IBusinessOperation CreateBusinessOperationObject(string name)
        {        
            string className = (string)classMaps[name];
            if (className == null)
            {
                throw new ArgumentNullException(String.Format("Operation not found [{0}]", name));
            }

            Assembly asm = Assembly.GetExecutingAssembly();
            IBusinessOperation obj = (IBusinessOperation)asm.CreateInstance(className);
            
            obj.SetNoSqlContext(noSqlContext);
            obj.SetStorageContext(storageContext);
            obj.SetSmtpContext(smtpContext);

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
    }
 
}