using System;
using System.Collections;
using System.Reflection;

using Magnum.Api.Commons.Business;
using Firebase.Database;

namespace Magnum.Api.Factories
{   
    public static class FactoryBusinessOperation
    {
        private static Hashtable classMaps = new Hashtable();
        private static FirebaseClient fbContext = null;

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
        }


        public static void SetContext(string url)
        {
            fbContext = new FirebaseClient(url);
        }

        public static void SetContext(FirebaseClient ctx)
        {
            fbContext = ctx;
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
            
            obj.SetContext(fbContext);

            return(obj);
        }
    }
 
}